using System.Globalization;
using System.Text;
using System.Text.Json;
using BitirmeBackend.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BitirmeBackend.Infrastructure.ExternalServices;

/// <summary>
/// Generates an action plan evaluation summary via the DeepSeek chat completions API
/// (OpenAI-compatible format). Failures and timeouts are swallowed and an empty string
/// is returned so PDF generation never breaks because of the LLM call.
/// </summary>
public class DeepSeekReportService : ILlmReportService
{
    private const string SystemPrompt =
        "Sen bir İK uzmanısın. Çalışan değerlendirme raporları için kısa, kişisel ve motive edici Türkçe metinler yazıyorsun. Metinler 3-4 paragraf olmalı, gereksiz uzun olmamalı. Resmi ama sıcak bir dil kullan.";

    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DeepSeekReportService> _logger;

    public DeepSeekReportService(
        HttpClient http,
        IConfiguration configuration,
        ILogger<DeepSeekReportService> logger)
    {
        _http = http;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GenerateActionPlanSummaryAsync(
        string employeeName,
        string department,
        string jobRole,
        List<(string CompetencyName, double Score)> competencyScores,
        List<string> actionItemTitles)
    {
        var apiKey = _configuration["DeepSeek:ApiKey"];
        _logger.LogInformation("DeepSeek ApiKey configured: {HasKey}", !string.IsNullOrEmpty(apiKey));
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogWarning(
                "DeepSeek API anahtarı (DeepSeek:ApiKey) yapılandırılmamış — API çağrısı yapılmadan değerlendirme metni atlanıyor.");
            return string.Empty;
        }

        var model = _configuration["DeepSeek:Model"] ?? "deepseek-v4-flash";
        var maxTokens = _configuration.GetValue<int>("DeepSeek:MaxTokens", 1000);

        var userPrompt = BuildUserPrompt(employeeName, department, jobRole, competencyScores, actionItemTitles);

        var payload = new
        {
            model,
            messages = new[]
            {
                new { role = "system", content = SystemPrompt },
                new { role = "user",   content = userPrompt }
            },
            max_tokens = maxTokens,
            temperature = 0.7,
            // deepseek-v4-flash enables thinking mode by default, which puts the answer
            // in "reasoning_content" and leaves "content" empty. Disable it so the text
            // comes back in "content". (We also read reasoning_content as a fallback.)
            thinking = new { type = "disabled" }
        };

        try
        {
            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, "/chat/completions")
            {
                Content = content
            };
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            _logger.LogInformation(
                "DeepSeek API çağrısı başlıyor... Model={Model}, BaseUrl={BaseUrl}",
                model, _http.BaseAddress);

            using var response = await _http.SendAsync(request);

            // Log status + raw body before doing anything else so we can diagnose
            // empty/unexpected responses.
            var rawContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("DeepSeek response status: {Status}", response.StatusCode);
            _logger.LogInformation("DeepSeek raw response: {Raw}", rawContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "DeepSeek isteği başarısız oldu. Status={Status}, Body={Body}",
                    response.StatusCode, rawContent);
                return string.Empty;
            }

            var summary = ParseContent(rawContent);

            _logger.LogInformation("DeepSeek yanıtı alındı, {Length} karakter", summary.Length);
            return summary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeepSeek değerlendirme metni üretilemedi — metin olmadan devam ediliyor.");
            return string.Empty;
        }
    }

    private static string BuildUserPrompt(
        string employeeName,
        string department,
        string jobRole,
        List<(string CompetencyName, double Score)> competencyScores,
        List<string> actionItemTitles)
    {
        var ci = CultureInfo.GetCultureInfo("tr-TR");

        var scoreLines = competencyScores.Count == 0
            ? "- (skor bulunmuyor)"
            : string.Join("\n", competencyScores.Select(s =>
                $"- {s.CompetencyName}: {s.Score.ToString("F1", ci)}"));

        var taskLines = actionItemTitles.Count == 0
            ? "- (görev bulunmuyor)"
            : string.Join("\n", actionItemTitles.Select(t => $"- {t}"));

        return
$@"Aşağıdaki bilgilere göre {employeeName} için kişisel bir gelişim değerlendirmesi yaz.

Çalışan: {employeeName}
Departman: {department}
Pozisyon: {jobRole}

Yetkinlik Skorları (5 üzerinden):
{scoreLines}

Oluşturulan Gelişim Görevleri:
{taskLines}

Değerlendirme şunları içermeli:
1. Çalışanın genel durumunu özetle (güçlü ve gelişim gerektiren alanlar)
2. Her göreve neden ihtiyaç duyulduğunu kısaca açıkla
3. Motive edici, pozitif ve kişisel bir kapanış cümlesi ekle

Türkçe yaz. 3-4 kısa paragraf. Başlık ekleme, direkt metne gir.";
    }

    private string ParseContent(string responseJson)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseJson);
            var message = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message");

            var content = message.TryGetProperty("content", out var contentEl)
                ? contentEl.GetString()
                : null;

            // Fallback: thinking mode puts the answer in "reasoning_content" and leaves
            // "content" empty. Read it if the main content is empty.
            if (string.IsNullOrWhiteSpace(content) &&
                message.TryGetProperty("reasoning_content", out var reasoningEl))
            {
                content = reasoningEl.GetString();
                if (!string.IsNullOrWhiteSpace(content))
                    _logger.LogInformation("DeepSeek content boş — reasoning_content alanına geçildi.");
            }

            return content?.Trim() ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeepSeek yanıtı ayrıştırılamadı. Raw={Raw}", responseJson);
            return string.Empty;
        }
    }
}
