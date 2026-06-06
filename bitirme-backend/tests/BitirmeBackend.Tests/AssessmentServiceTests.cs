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

    [Fact]
    public async Task UpsertAssessmentScoreAsync_CompletedAssessment_ThrowsArgumentException()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Completed });

        await svc.Invoking(s => s.UpsertAssessmentScoreAsync(1,
                new UpsertAssessmentScoreRequest { CompetencyId = 1, EvaluatorType = "Manager", Score = 3.0 }))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Tamamlanmış değerlendirmede*");

        assessments.Verify(r => r.AddScoreAsync(It.IsAny<AssessmentScore>()), Times.Never);
    }

    [Fact]
    public async Task UpsertAssessmentScoreAsync_DraftAssessment_CreatesScore()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(1))
                   .ReturnsAsync(new Assessment { Id = 1, EmployeeId = 10, Status = AssessmentStatus.Draft });
        assessments.Setup(r => r.GetScoreAsync(1, 1, "Manager")).ReturnsAsync((AssessmentScore?)null);
        assessments.Setup(r => r.AddScoreAsync(It.IsAny<AssessmentScore>())).Returns(Task.CompletedTask);
        assessments.Setup(r => r.GetScoresByAssessmentIdAsync(1))
                   .ReturnsAsync(new List<AssessmentScore>());

        var result = await svc.UpsertAssessmentScoreAsync(1,
            new UpsertAssessmentScoreRequest { CompetencyId = 1, EvaluatorType = "Manager", Score = 3.0 });

        result.Should().NotBeNull();
        assessments.Verify(r => r.AddScoreAsync(It.IsAny<AssessmentScore>()), Times.Once);
    }

    [Fact]
    public async Task UpsertAssessmentScoreAsync_AssessmentNotFound_ThrowsKeyNotFound()
    {
        var (svc, assessments, _) = Build();
        assessments.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Assessment?)null);

        await svc.Invoking(s => s.UpsertAssessmentScoreAsync(99,
                new UpsertAssessmentScoreRequest { CompetencyId = 1, EvaluatorType = "Manager", Score = 3.0 }))
            .Should().ThrowAsync<KeyNotFoundException>();
    }
}
