using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Services;
using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Domain.Enums;
using FluentAssertions;
using Moq;

namespace BitirmeBackend.Tests;

public class AssessmentServiceTests
{
    private static (AssessmentService Svc,
                    Mock<IAssessmentRepository> Assessments,
                    Mock<IActionPlanRepository> ActionPlans) Build()
    {
        var assessments = new Mock<IAssessmentRepository>();
        var actionPlans = new Mock<IActionPlanRepository>();
        var svc = new AssessmentService(assessments.Object, actionPlans.Object);
        return (svc, assessments, actionPlans);
    }

    private static AssessmentAssignment Assignment(int evaluatorId, bool completed) => new()
    {
        Id = evaluatorId, AssessmentId = 1, EvaluatorEmployeeId = evaluatorId,
        EvaluatorType = "Peer", IsCompleted = completed
    };

    // ── UpsertAssessmentScoreAsync ────────────────────────────────────────────

    [Fact]
    public async Task UpsertAssessmentScoreAsync_DraftAssessment_CreatesScore()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft });
        assessments.Setup(r => r.GetAssignmentAsync(1, 5)).ReturnsAsync(Assignment(5, completed: false));
        assessments.Setup(r => r.GetScoreAsync(1, 1, 5)).ReturnsAsync((AssessmentScore?)null);
        assessments.Setup(r => r.AddScoreAsync(It.IsAny<AssessmentScore>())).Returns(Task.CompletedTask);
        assessments.Setup(r => r.GetScoresByAssessmentIdAsync(1)).ReturnsAsync(new List<AssessmentScore>());

        var result = await svc.UpsertAssessmentScoreAsync(1,
            new UpsertAssessmentScoreRequest { CompetencyId = 1, EvaluatorEmployeeId = 5, EvaluatorType = "Peer", Score = 3.0 });

        result.Should().NotBeNull();
        assessments.Verify(r => r.AddScoreAsync(It.IsAny<AssessmentScore>()), Times.Once);
    }

    [Fact]
    public async Task UpsertAssessmentScoreAsync_AssessmentNotFound_ThrowsKeyNotFound()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Assessment?)null);

        await svc.Invoking(s => s.UpsertAssessmentScoreAsync(99,
                new UpsertAssessmentScoreRequest { CompetencyId = 1, EvaluatorEmployeeId = 5, EvaluatorType = "Peer", Score = 3.0 }))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task UpsertScore_NoAssignment_ThrowsArgumentException()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft });
        assessments.Setup(r => r.GetAssignmentAsync(1, 5)).ReturnsAsync((AssessmentAssignment?)null);

        await svc.Invoking(s => s.UpsertAssessmentScoreAsync(1,
                new UpsertAssessmentScoreRequest { CompetencyId = 1, EvaluatorEmployeeId = 5, EvaluatorType = "Peer", Score = 3.0 }))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*atama bulunamadı*");

        assessments.Verify(r => r.AddScoreAsync(It.IsAny<AssessmentScore>()), Times.Never);
    }

    [Fact]
    public async Task UpsertScore_AlreadyCompleted_ThrowsArgumentException()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Completed });
        assessments.Setup(r => r.GetAssignmentAsync(1, 2)).ReturnsAsync(Assignment(2, completed: true));

        await svc.Invoking(s => s.UpsertAssessmentScoreAsync(1,
                new UpsertAssessmentScoreRequest { CompetencyId = 1, EvaluatorEmployeeId = 2, EvaluatorType = "Manager", Score = 3.0 }))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*zaten tamamladınız*");

        assessments.Verify(r => r.AddScoreAsync(It.IsAny<AssessmentScore>()), Times.Never);
    }

    [Fact]
    public async Task UpsertScore_ThirteenthCompetency_MarksAssignmentCompleted()
    {
        var (svc, assessments, _) = Build();
        var assignment = Assignment(5, completed: false);

        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft });
        assessments.Setup(r => r.GetAssignmentAsync(1, 5)).ReturnsAsync(assignment);
        assessments.Setup(r => r.GetScoreAsync(1, 13, 5)).ReturnsAsync((AssessmentScore?)null);
        assessments.Setup(r => r.AddScoreAsync(It.IsAny<AssessmentScore>())).Returns(Task.CompletedTask);

        // After saving, the evaluator now has all 13 competencies scored.
        var thirteen = Enumerable.Range(1, 13)
            .Select(c => new AssessmentScore { Id = c, AssessmentId = 1, CompetencyId = c, EvaluatorEmployeeId = 5 })
            .ToList();
        assessments.Setup(r => r.GetScoresByAssessmentIdAsync(1)).ReturnsAsync(thirteen);
        // Completing the survey triggers the FIX 1 auto-complete check.
        assessments.Setup(r => r.GetAssignmentsByAssessmentAsync(1))
                   .ReturnsAsync(new List<AssessmentAssignment> { assignment });

        await svc.UpsertAssessmentScoreAsync(1,
            new UpsertAssessmentScoreRequest { CompetencyId = 13, EvaluatorEmployeeId = 5, EvaluatorType = "Peer", Score = 3.0 });

        assignment.IsCompleted.Should().BeTrue();
        assignment.CompletedAt.Should().NotBeNull();
        assessments.Verify(r => r.UpdateAssignment(assignment), Times.Once);
    }

    // ── CreateAssignmentAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task CreateAssignment_Duplicate_ThrowsArgumentException()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft });
        assessments.Setup(r => r.GetAssignmentAsync(1, 3)).ReturnsAsync(Assignment(3, completed: false));

        await svc.Invoking(s => s.CreateAssignmentAsync(
                new CreateAssessmentAssignmentRequest { AssessmentId = 1, EvaluatorEmployeeId = 3, EvaluatorType = "Peer" }, 2))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*zaten atanmış*");

        assessments.Verify(r => r.AddAssignmentAsync(It.IsAny<AssessmentAssignment>()), Times.Never);
    }

    [Fact]
    public async Task CreateAssignment_New_AddsAssignment()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft });
        assessments.SetupSequence(r => r.GetAssignmentAsync(1, 4))
                   .ReturnsAsync((AssessmentAssignment?)null)               // duplicate check
                   .ReturnsAsync(Assignment(4, completed: false));          // reload after add
        assessments.Setup(r => r.AddAssignmentAsync(It.IsAny<AssessmentAssignment>())).Returns(Task.CompletedTask);

        var result = await svc.CreateAssignmentAsync(
            new CreateAssessmentAssignmentRequest { AssessmentId = 1, EvaluatorEmployeeId = 4, EvaluatorType = "Peer" }, 2);

        result.EvaluatorEmployeeId.Should().Be(4);
        assessments.Verify(r => r.AddAssignmentAsync(It.IsAny<AssessmentAssignment>()), Times.Once);
    }

    // ── GetMySurveysAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetMySurveys_ReturnsPendingAssignments()
    {
        var (svc, assessments, _) = Build();

        var assessment = new Assessment
        {
            Id = 1, EmployeeId = 10, CycleId = 1, Status = AssessmentStatus.Completed,
            Employee = new Employee { Id = 10, FullName = "Ayşe Kaya" },
            Cycle = new AssessmentCycle { Id = 1, Name = "2025 Q4" }
        };

        var pending = new AssessmentAssignment
        {
            Id = 4, AssessmentId = 1, EvaluatorEmployeeId = 4, EvaluatorType = "Peer",
            IsCompleted = false, Assessment = assessment
        };
        var done = new AssessmentAssignment
        {
            Id = 5, AssessmentId = 1, EvaluatorEmployeeId = 4, EvaluatorType = "Peer",
            IsCompleted = true, Assessment = assessment
        };

        assessments.Setup(r => r.GetAssignmentsByEvaluatorAsync(4))
                   .ReturnsAsync(new List<AssessmentAssignment> { pending, done });

        var result = await svc.GetMySurveysAsync(4);

        result.Should().HaveCount(1);
        result[0].AssignmentId.Should().Be(4);
        result[0].EmployeeName.Should().Be("Ayşe Kaya");
        result[0].CycleName.Should().Be("2025 Q4");
        result[0].CompetencyCount.Should().Be(13);
    }

    // ── FIX 1: auto-complete assessment ───────────────────────────────────────

    [Fact]
    public async Task AutoComplete_WhenAllAssignmentsDone_SetsAssessmentCompleted()
    {
        var (svc, assessments, _) = Build();
        var assignment = Assignment(5, completed: false);
        var assessment = new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft };

        assessments.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(assessment);
        assessments.Setup(r => r.GetAssignmentAsync(1, 5)).ReturnsAsync(assignment);
        assessments.Setup(r => r.GetScoreAsync(1, 13, 5)).ReturnsAsync((AssessmentScore?)null);
        assessments.Setup(r => r.AddScoreAsync(It.IsAny<AssessmentScore>())).Returns(Task.CompletedTask);

        var thirteen = Enumerable.Range(1, 13)
            .Select(c => new AssessmentScore { Id = c, AssessmentId = 1, CompetencyId = c, EvaluatorEmployeeId = 5 })
            .ToList();
        assessments.Setup(r => r.GetScoresByAssessmentIdAsync(1)).ReturnsAsync(thirteen);
        // The only assignment is the one we just completed → assessment should auto-complete.
        assessments.Setup(r => r.GetAssignmentsByAssessmentAsync(1))
                   .ReturnsAsync(new List<AssessmentAssignment> { assignment });

        await svc.UpsertAssessmentScoreAsync(1,
            new UpsertAssessmentScoreRequest { CompetencyId = 13, EvaluatorEmployeeId = 5, EvaluatorType = "Peer", Score = 3.0 });

        assessment.Status.Should().Be(AssessmentStatus.Completed);
        assessments.Verify(r => r.Update(assessment), Times.Once);
    }

    // ── FIX 3: Self/Manager uniqueness ────────────────────────────────────────

    [Fact]
    public async Task CreateAssignment_DuplicateSelf_ThrowsArgumentException()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft });
        assessments.Setup(r => r.GetAssignmentAsync(1, 7)).ReturnsAsync((AssessmentAssignment?)null);
        assessments.Setup(r => r.GetAssignmentsByAssessmentAsync(1))
                   .ReturnsAsync(new List<AssessmentAssignment>
                   {
                       new() { Id = 1, AssessmentId = 1, EvaluatorEmployeeId = 10, EvaluatorType = "Self", IsCompleted = false }
                   });

        await svc.Invoking(s => s.CreateAssignmentAsync(
                new CreateAssessmentAssignmentRequest { AssessmentId = 1, EvaluatorEmployeeId = 7, EvaluatorType = "Self" }, 2))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Self ataması zaten mevcut*");

        assessments.Verify(r => r.AddAssignmentAsync(It.IsAny<AssessmentAssignment>()), Times.Never);
    }

    [Fact]
    public async Task CreateAssignment_DuplicateManager_ThrowsArgumentException()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft });
        assessments.Setup(r => r.GetAssignmentAsync(1, 8)).ReturnsAsync((AssessmentAssignment?)null);
        assessments.Setup(r => r.GetAssignmentsByAssessmentAsync(1))
                   .ReturnsAsync(new List<AssessmentAssignment>
                   {
                       new() { Id = 2, AssessmentId = 1, EvaluatorEmployeeId = 2, EvaluatorType = "Manager", IsCompleted = false }
                   });

        await svc.Invoking(s => s.CreateAssignmentAsync(
                new CreateAssessmentAssignmentRequest { AssessmentId = 1, EvaluatorEmployeeId = 8, EvaluatorType = "Manager" }, 2))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Manager ataması zaten mevcut*");

        assessments.Verify(r => r.AddAssignmentAsync(It.IsAny<AssessmentAssignment>()), Times.Never);
    }

    // ── FIX 5: bulk score submission ──────────────────────────────────────────

    [Fact]
    public async Task BulkUpsert_ValidScores_MarksAssignmentComplete()
    {
        var (svc, assessments, _) = Build();
        var assignment = Assignment(5, completed: false);
        var assessment = new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft };

        assessments.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(assessment);
        assessments.Setup(r => r.GetAssignmentAsync(1, 5)).ReturnsAsync(assignment);
        assessments.Setup(r => r.GetScoreAsync(1, It.IsAny<int>(), 5)).ReturnsAsync((AssessmentScore?)null);
        assessments.Setup(r => r.AddScoreAsync(It.IsAny<AssessmentScore>())).Returns(Task.CompletedTask);

        var thirteen = Enumerable.Range(1, 13)
            .Select(c => new AssessmentScore { Id = c, AssessmentId = 1, CompetencyId = c, EvaluatorEmployeeId = 5 })
            .ToList();
        assessments.Setup(r => r.GetScoresByAssessmentIdAsync(1)).ReturnsAsync(thirteen);
        assessments.Setup(r => r.GetAssignmentsByAssessmentAsync(1))
                   .ReturnsAsync(new List<AssessmentAssignment> { assignment });

        var request = new BulkUpsertAssessmentScoreRequest
        {
            EvaluatorEmployeeId = 5,
            Scores = Enumerable.Range(1, 13)
                .Select(c => new BulkUpsertAssessmentScoreRequest.ScoreItem { CompetencyId = c, Score = 3.0 })
                .ToList()
        };

        var result = await svc.BulkUpsertScoresAsync(1, request);

        result.IsCompleted.Should().BeTrue();
        assignment.IsCompleted.Should().BeTrue();
        assessments.Verify(r => r.AddScoreAsync(It.IsAny<AssessmentScore>()), Times.Exactly(13));
    }

    [Fact]
    public async Task BulkUpsert_AlreadyCompleted_ThrowsArgumentException()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Completed });
        assessments.Setup(r => r.GetAssignmentAsync(1, 2)).ReturnsAsync(Assignment(2, completed: true));

        var request = new BulkUpsertAssessmentScoreRequest
        {
            EvaluatorEmployeeId = 2,
            Scores = new List<BulkUpsertAssessmentScoreRequest.ScoreItem>
            {
                new() { CompetencyId = 1, Score = 3.0 }
            }
        };

        await svc.Invoking(s => s.BulkUpsertScoresAsync(1, request))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*zaten tamamladınız*");

        assessments.Verify(r => r.AddScoreAsync(It.IsAny<AssessmentScore>()), Times.Never);
    }
}
