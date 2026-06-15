using BitirmeBackend.Application.Interfaces;
using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BitirmeBackend.Infrastructure.Pdf;

public class PdfExportService : IPdfExportService
{
    static PdfExportService()
    {
        // Community license — must be set once before any PDF is generated
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    }

    // ── Brand palette (hex strings accepted by QuestPDF) ───────────────────────
    private const string Primary    = "#1F3C56"; // navy
    private const string PrimaryDk  = "#142A3D";
    private const string Accent     = "#2563EB"; // blue
    private const string SurfaceLt  = "#F1F5F9"; // light slate
    private const string Border     = "#E2E8F0";
    private const string TextMuted  = "#64748B";
    private const string TextDark   = "#1E293B";

    private const string HighColor   = "#DC2626"; // red
    private const string MediumColor = "#D97706"; // amber
    private const string LowColor    = "#16A34A"; // green

    private readonly IActionPlanRepository _actionPlans;
    private readonly IAssessmentRepository _assessments;
    private readonly IEmployeeTaskRepository _employeeTasks;
    private readonly ILlmReportService _llmReportService;

    public PdfExportService(
        IActionPlanRepository actionPlans,
        IAssessmentRepository assessments,
        IEmployeeTaskRepository employeeTasks,
        ILlmReportService llmReportService)
    {
        _actionPlans      = actionPlans;
        _assessments      = assessments;
        _employeeTasks    = employeeTasks;
        _llmReportService = llmReportService;
    }

    public async Task<byte[]> GenerateActionPlanPdfAsync(int actionPlanId)
    {
        // 1. Load plan + items
        var plan = await _actionPlans.GetByIdWithItemsAsync(actionPlanId)
            ?? throw new KeyNotFoundException($"Aksiyon planı bulunamadı: {actionPlanId}");

        // 2. Load assessment (for cycle name)
        var assessment = await _assessments.GetByIdWithDetailsAsync(plan.AssessmentId);

        // 3. Active items only — completed last, then priority High→Medium→Low, then OrderNo
        var items = plan.Items
            .Where(i => !i.IsDeleted)
            .OrderBy(i => i.EmployeeTasks.Any(t => !t.IsDeleted && t.Status == EmployeeTaskStatus.Completed) ? 1 : 0)
            .ThenByDescending(i => i.Priority)
            .ThenBy(i => i.OrderNo)
            .ToList();

        // 4. Load EmployeeTasks for each item
        var taskMap = new Dictionary<int, EmployeeTask?>();
        foreach (var item in items)
        {
            var tasks = (await _employeeTasks.GetByActionPlanItemIdAsync(item.Id)).ToList();
            taskMap[item.Id] = tasks.FirstOrDefault();
        }

        // 5. Resolve header data
        var employee     = plan.Employee;
        var department   = employee?.Department?.Name ?? "-";
        var jobRole      = employee?.JobRole?.Name    ?? "-";
        var employeeCode = string.IsNullOrWhiteSpace(employee?.EmployeeCode) ? "-" : employee!.EmployeeCode;
        var managerName  = string.IsNullOrWhiteSpace(employee?.Manager?.FullName) ? "-" : employee!.Manager!.FullName;
        var cycleName    = assessment?.Cycle?.Name ?? $"Değerlendirme #{plan.AssessmentId}";
        var employeeName = employee?.FullName ?? "-";

        // 6. Summary statistics
        var statuses = items.Select(i =>
        {
            taskMap.TryGetValue(i.Id, out var t);
            return t?.Status;
        }).ToList();

        int total      = items.Count;
        int completed  = statuses.Count(s => s == EmployeeTaskStatus.Completed);
        int inProgress = statuses.Count(s => s == EmployeeTaskStatus.InProgress);
        int pending    = statuses.Count(s => s is null or EmployeeTaskStatus.Pending);
        int high       = items.Count(i => i.Priority == PriorityLevel.High);
        int progressPct = total == 0 ? 0 : (int)Math.Round(completed * 100.0 / total);

        // 7. Build LLM evaluation summary (best-effort — empty string skips the section)
        var competencyScores = (assessment?.Scores ?? Enumerable.Empty<AssessmentScore>())
            .Where(s => !s.IsDeleted)
            .GroupBy(s => new { s.CompetencyId, Name = s.Competency?.Name ?? string.Empty })
            .Select(g => (CompetencyName: g.Key.Name, Score: g.Average(s => s.Score)))
            .Where(x => !string.IsNullOrWhiteSpace(x.CompetencyName))
            .ToList();

        var actionItemTitles = items.Select(i => i.Title).ToList();

        var summaryText = await _llmReportService.GenerateActionPlanSummaryAsync(
            employeeName, department, jobRole, competencyScores, actionItemTitles);

        // Single timestamp shared by every page footer for consistency
        var generatedAt = DateTime.UtcNow;

        // Explicit pagination: page 1 carries the intro layout + first item,
        // every subsequent page holds exactly 4 items. Each card uses ShowEntire()
        // so it is never split internally across a page boundary.
        var firstItem  = items.Count > 0 ? items[0] : null;
        var laterItems = items.Skip(1).ToList();

        var pdfBytes = Document.Create(container =>
        {
            // ── PAGE 1 — intro layout + first item only ──────────────────────
            container.Page(page =>
            {
                ConfigurePage(page);
                page.Header().Element(c => PageHeader(c, employeeName, cycleName));

                page.Content().PaddingHorizontal(40).PaddingVertical(18).Column(col =>
                {
                    // Employee info card
                    col.Item().Element(c => EmployeeInfoCard(c,
                        employeeName, employeeCode, department, jobRole, managerName,
                        cycleName, plan));

                    // Summary statistics strip
                    col.Item().PaddingTop(16).Element(c => SummaryStrip(c,
                        total, completed, inProgress, pending, high, progressPct));

                    // AI-generated personalized evaluation (only when LLM returned text)
                    if (!string.IsNullOrWhiteSpace(summaryText))
                    {
                        col.Item().PaddingTop(20).PaddingBottom(8).Row(r =>
                        {
                            r.ConstantItem(4).Background(Accent);
                            r.ConstantItem(8);
                            r.RelativeItem().AlignMiddle().Text("Kişisel Gelişim Değerlendirmesi")
                                .FontSize(14).Bold().FontColor(Primary);
                        });

                        col.Item().PaddingBottom(8).Text(summaryText)
                            .FontSize(10).FontColor(TextDark).LineHeight(1.4f);
                    }

                    // Section title
                    col.Item().PaddingTop(20).PaddingBottom(8).Row(r =>
                    {
                        r.ConstantItem(4).Background(Accent);
                        r.ConstantItem(8);
                        r.RelativeItem().AlignMiddle().Text("Gelişim Aksiyonları")
                            .FontSize(14).Bold().FontColor(Primary);
                    });

                    if (firstItem is null)
                    {
                        col.Item().PaddingTop(8).Background(SurfaceLt).Padding(16)
                            .Text("Bu plana ait aktif aksiyon bulunmuyor.")
                            .Italic().FontColor(TextMuted);
                    }
                    else
                    {
                        taskMap.TryGetValue(firstItem.Id, out var firstTask);
                        col.Item().PaddingBottom(12).ShowEntire()
                            .Element(c => ActionCard(c, 1, firstItem, firstTask));
                    }
                });

                page.Footer().Element(c => PageFooter(c, generatedAt));
            });

            // ── PAGES 2+ — exactly 4 items per page ──────────────────────────
            for (int offset = 0; offset < laterItems.Count; offset += 4)
            {
                var group       = laterItems.Skip(offset).Take(4).ToList();
                var firstNumber = offset + 2; // continuous numbering after the page-1 item (#1)

                container.Page(page =>
                {
                    ConfigurePage(page);
                    page.Header().Element(c => PageHeader(c, employeeName, cycleName));

                    page.Content().PaddingHorizontal(40).PaddingVertical(18).Column(col =>
                    {
                        int n = firstNumber;
                        foreach (var item in group)
                        {
                            taskMap.TryGetValue(item.Id, out var empTask);
                            var captured     = item;
                            var capturedTask = empTask;
                            var num          = n++;
                            col.Item().PaddingBottom(12).ShowEntire()
                                .Element(c => ActionCard(c, num, captured, capturedTask));
                        }
                    });

                    page.Footer().Element(c => PageFooter(c, generatedAt));
                });
            }
        }).GeneratePdf();

        return pdfBytes;
    }

    // ── Page scaffolding (shared across all explicit pages) ─────────────────────

    private static void ConfigurePage(PageDescriptor page)
    {
        page.Size(PageSizes.A4);
        page.Margin(0); // full-bleed header/footer; inner padding applied manually
        page.DefaultTextStyle(t => t.FontFamily("Noto Sans").FontSize(10).FontColor(TextDark));
    }

    private void PageHeader(IContainer container, string employeeName, string cycleName)
    {
        container.Background(Primary).PaddingHorizontal(40).PaddingVertical(16).Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("Kişisel Gelişim Aksiyon Planı")
                    .FontSize(18).Bold().FontColor(Colors.White);
                col.Item().PaddingTop(2).Text("Bireysel Yetkinlik Geliştirme Raporu")
                    .FontSize(9).FontColor("#AFC4D9");
            });

            row.ConstantItem(170).AlignRight().Column(col =>
            {
                col.Item().AlignRight().Text(employeeName)
                    .FontSize(11).Bold().FontColor(Colors.White);
                col.Item().AlignRight().Text(cycleName)
                    .FontSize(9).FontColor("#AFC4D9");
            });
        });
    }

    private void PageFooter(IContainer container, DateTime generatedAt)
    {
        container.BorderTop(1).BorderColor(Border).PaddingHorizontal(40).PaddingVertical(8).Row(row =>
        {
            row.RelativeItem().Column(c =>
            {
                c.Item().Text($"Oluşturulma: {generatedAt:dd.MM.yyyy HH:mm} (UTC)")
                    .FontSize(8).FontColor(TextMuted);
                c.Item().Text("Bu belge gizlidir ve yalnızca ilgili çalışan ve yöneticisi için hazırlanmıştır.")
                    .FontSize(8).FontColor(TextMuted);
            });

            row.ConstantItem(70).AlignRight().AlignBottom().Text(txt =>
            {
                txt.DefaultTextStyle(t => t.FontSize(8).FontColor(TextMuted));
                txt.Span("Sayfa ");
                txt.CurrentPageNumber();
                txt.Span(" / ");
                txt.TotalPages();
            });
        });
    }

    // ── Composite sections ─────────────────────────────────────────────────────

    private void EmployeeInfoCard(IContainer container,
        string name, string code, string dept, string role, string manager,
        string cycle, ActionPlan plan)
    {
        container.Background(SurfaceLt).Border(1).BorderColor(Border).Padding(16).Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                InfoLine(col, "Çalışan",     name);
                InfoLine(col, "Çalışan Kodu", code);
                InfoLine(col, "Departman",   dept);
                InfoLine(col, "Pozisyon",    role);
            });

            row.ConstantItem(24);

            row.RelativeItem().Column(col =>
            {
                InfoLine(col, "Yönetici",            manager);
                InfoLine(col, "Değerlendirme Dönemi", cycle);
                InfoLine(col, "Oluşturma Tarihi",    plan.CreatedAt.ToString("dd.MM.yyyy"));
                InfoLine(col, "Onay Tarihi",         plan.ApprovedAt?.ToString("dd.MM.yyyy") ?? "-");

                col.Item().PaddingTop(6).Row(r =>
                {
                    r.AutoItem().Text("Plan Durumu:").FontSize(9).FontColor(TextMuted);
                    r.ConstantItem(6);
                    r.AutoItem().Element(c => Badge(c, MapStatus(plan.Status), PlanStatusColor(plan.Status)));
                });
            });
        });
    }

    private static void InfoLine(ColumnDescriptor col, string label, string value)
    {
        col.Item().PaddingVertical(2).Row(row =>
        {
            row.ConstantItem(110).Text(label).FontSize(9).FontColor(TextMuted);
            row.RelativeItem().Text(value).FontSize(10).SemiBold().FontColor(TextDark);
        });
    }

    private void SummaryStrip(IContainer container,
        int total, int completed, int inProgress, int pending, int high, int progressPct)
    {
        container.Row(row =>
        {
            row.Spacing(10);
            StatBox(row, "Toplam Aksiyon", total.ToString(),       Primary);
            StatBox(row, "Tamamlanan",     completed.ToString(),   LowColor);
            StatBox(row, "Devam Eden",     inProgress.ToString(),  Accent);
            StatBox(row, "Bekleyen",       pending.ToString(),     TextMuted);
            StatBox(row, "Yüksek Öncelik", high.ToString(),        HighColor);
            StatBox(row, "İlerleme",       $"%{progressPct}",      PrimaryDk);
        });
    }

    private static void StatBox(RowDescriptor row, string label, string value, string accent)
    {
        row.RelativeItem().Border(1).BorderColor(Border).Background(Colors.White).Column(col =>
        {
            col.Item().Height(4).Background(accent);
            col.Item().PaddingVertical(8).PaddingHorizontal(6).Column(inner =>
            {
                inner.Item().AlignCenter().Text(value).FontSize(18).Bold().FontColor(accent);
                inner.Item().AlignCenter().Text(label).FontSize(8).FontColor(TextMuted);
            });
        });
    }

    private void ActionCard(IContainer container, int index, ActionPlanItem item, EmployeeTask? task)
    {
        var priorityColor = PriorityColor(item.Priority);

        container.Border(1).BorderColor(Border).Background(Colors.White).Column(card =>
        {
            // Header band of the card
            card.Item().Background(SurfaceLt).BorderBottom(1).BorderColor(Border)
                .PaddingVertical(8).PaddingHorizontal(12).Row(row =>
            {
                // Number badge
                row.ConstantItem(28).AlignMiddle().Height(24).Background(Primary)
                    .AlignCenter().AlignMiddle()
                    .Text(index.ToString()).FontColor(Colors.White).Bold().FontSize(11);

                row.ConstantItem(10);

                row.RelativeItem().AlignMiddle().Text(item.Title)
                    .FontSize(12).Bold().FontColor(Primary);

                // Priority + status badges
                row.AutoItem().AlignMiddle().Element(c => Badge(c, MapPriority(item.Priority), priorityColor));
                row.ConstantItem(6);
                row.AutoItem().AlignMiddle().Element(c =>
                {
                    var status = task?.Status;
                    Badge(c, status is not null ? MapTaskStatus(status.Value) : "Atanmadı",
                          status is not null ? TaskStatusColor(status.Value) : TextMuted);
                });
            });

            // Body
            card.Item().Padding(12).Column(body =>
            {
                if (!string.IsNullOrWhiteSpace(item.Description))
                {
                    body.Item().Text(item.Description).FontSize(10).FontColor(TextDark).LineHeight(1.35f);
                }

                // Meta grid
                body.Item().PaddingTop(item.Description is { Length: > 0 } ? 10 : 0).Row(row =>
                {
                    row.Spacing(16);
                    MetaItem(row, "Kaynak Türü",  MapSource(item.Source));
                    MetaItem(row, "Format",       string.IsNullOrWhiteSpace(item.DeliveryType) ? "-" : item.DeliveryType!);
                    MetaItem(row, "Son Tarih",    item.DueDate?.ToString("dd.MM.yyyy")
                                                  ?? task?.DueDate?.ToString("dd.MM.yyyy") ?? "-");
                    MetaItem(row, "Tamamlanma",   task?.CompletedAt?.ToString("dd.MM.yyyy") ?? "-");
                });

                // Resource link (clickable)
                if (!string.IsNullOrWhiteSpace(item.Resource))
                {
                    body.Item().PaddingTop(10).BorderTop(1).BorderColor(Border).PaddingTop(8).Row(row =>
                    {
                        row.AutoItem().AlignMiddle().Text("🔗 Kaynak:")
                            .FontSize(9).SemiBold().FontColor(TextMuted);
                        row.ConstantItem(6);
                        row.RelativeItem().AlignMiddle().Text(txt =>
                        {
                            var url = item.Resource!.Trim();
                            txt.Hyperlink(url, AbsoluteUrl(url))
                                .FontSize(9).FontColor(Accent).Underline();
                        });
                    });
                }
            });
        });
    }

    private static void MetaItem(RowDescriptor row, string label, string value)
    {
        row.RelativeItem().Column(col =>
        {
            col.Item().Text(label).FontSize(8).FontColor(TextMuted);
            col.Item().Text(value).FontSize(9).SemiBold().FontColor(TextDark);
        });
    }

    private static void Badge(IContainer container, string text, string color)
    {
        container.Background(color).PaddingHorizontal(8).PaddingVertical(3)
            .Text(text).FontSize(8).Bold().FontColor(Colors.White);
    }

    private static string AbsoluteUrl(string url) =>
        url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
        url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            ? url
            : $"https://{url}";

    // ── Mapping helpers ────────────────────────────────────────────────────────

    private static string MapStatus(ActionPlanStatus s) => s switch
    {
        ActionPlanStatus.Draft     => "Taslak",
        ActionPlanStatus.Edited    => "Düzenlendi",
        ActionPlanStatus.Approved  => "Onaylandı",
        ActionPlanStatus.Sent      => "Gönderildi",
        ActionPlanStatus.Completed => "Tamamlandı",
        ActionPlanStatus.Cancelled => "İptal Edildi",
        _                          => s.ToString()
    };

    private static string PlanStatusColor(ActionPlanStatus s) => s switch
    {
        ActionPlanStatus.Draft     => TextMuted,
        ActionPlanStatus.Edited    => MediumColor,
        ActionPlanStatus.Approved  => Accent,
        ActionPlanStatus.Sent      => "#0D9488",
        ActionPlanStatus.Completed => LowColor,
        ActionPlanStatus.Cancelled => HighColor,
        _                          => TextMuted
    };

    private static string MapPriority(PriorityLevel p) => p switch
    {
        PriorityLevel.High   => "Yüksek",
        PriorityLevel.Medium => "Orta",
        PriorityLevel.Low    => "Düşük",
        _                    => p.ToString()
    };

    private static string PriorityColor(PriorityLevel p) => p switch
    {
        PriorityLevel.High   => HighColor,
        PriorityLevel.Medium => MediumColor,
        PriorityLevel.Low    => LowColor,
        _                    => TextMuted
    };

    private static string MapSource(ActionPlanItemSource src) => src switch
    {
        ActionPlanItemSource.AI       => "AI Önerisi",
        ActionPlanItemSource.Manual   => "Manuel",
        ActionPlanItemSource.EditedAI => "Düzenlenmiş AI",
        _                             => src.ToString()
    };

    private static string MapTaskStatus(EmployeeTaskStatus s) => s switch
    {
        EmployeeTaskStatus.Pending    => "Beklemede",
        EmployeeTaskStatus.InProgress => "Devam Ediyor",
        EmployeeTaskStatus.Completed  => "Tamamlandı",
        EmployeeTaskStatus.Cancelled  => "İptal",
        _                             => s.ToString()
    };

    private static string TaskStatusColor(EmployeeTaskStatus s) => s switch
    {
        EmployeeTaskStatus.Pending    => TextMuted,
        EmployeeTaskStatus.InProgress => Accent,
        EmployeeTaskStatus.Completed  => LowColor,
        EmployeeTaskStatus.Cancelled  => HighColor,
        _                             => TextMuted
    };
}
