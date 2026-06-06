using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;

namespace BitirmeBackend.Infrastructure.MockRepositories;

public class MockAssessmentRepository : IAssessmentRepository
{
    private static IEnumerable<Assessment> Active =>
        MockDataStore.Assessments.Where(a => !a.IsDeleted);

    public Task<Assessment?> GetByIdAsync(int id) =>
        Task.FromResult(Active.FirstOrDefault(a => a.Id == id));

    public Task<Assessment?> GetByIdWithDetailsAsync(int id)
    {
        var a = Active.FirstOrDefault(x => x.Id == id);
        if (a is not null)
        {
            a.Employee       = MockDataStore.Employees.FirstOrDefault(e => e.Id == a.EmployeeId)!;
            a.Cycle          = MockDataStore.AssessmentCycles.FirstOrDefault(c => c.Id == a.CycleId)!;
            a.CreatedByUser  = MockDataStore.Users.FirstOrDefault(u => u.Id == a.CreatedByUserId)!;
            a.Scores         = MockDataStore.AssessmentScores.Where(s => s.AssessmentId == a.Id).ToList();
        }
        return Task.FromResult(a);
    }

    public Task<(IEnumerable<Assessment> Items, int TotalCount)> GetByEmployeePagedAsync(
        int employeeId, int pageNumber, int pageSize)
    {
        var all = Active.Where(a => a.EmployeeId == employeeId).ToList();
        var items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return Task.FromResult((items, all.Count));
    }

    public Task<IEnumerable<AssessmentScore>> GetScoresByAssessmentIdAsync(int assessmentId)
    {
        var scores = MockDataStore.AssessmentScores
            .Where(s => s.AssessmentId == assessmentId && !s.IsDeleted)
            .ToList();

        // Attach competency navigation
        foreach (var s in scores)
            s.Competency = MockDataStore.Competencies.FirstOrDefault(c => c.Id == s.CompetencyId)!;

        return Task.FromResult<IEnumerable<AssessmentScore>>(scores);
    }

    public Task<AssessmentScore?> GetScoreAsync(int assessmentId, int competencyId, string evaluatorType) =>
        Task.FromResult(MockDataStore.AssessmentScores.FirstOrDefault(s =>
            s.AssessmentId == assessmentId &&
            s.CompetencyId == competencyId &&
            s.EvaluatorType.ToString() == evaluatorType &&
            !s.IsDeleted));

    public Task<AssessmentScore?> GetScoreAsync(int assessmentId, int competencyId, int evaluatorEmployeeId) =>
        Task.FromResult(MockDataStore.AssessmentScores.FirstOrDefault(s =>
            s.AssessmentId == assessmentId &&
            s.CompetencyId == competencyId &&
            s.EvaluatorEmployeeId == evaluatorEmployeeId &&
            !s.IsDeleted));

    public Task AddAsync(Assessment assessment)
    {
        assessment.Id = MockDataStore.NextAssessmentId++;
        assessment.CreatedAt = DateTime.UtcNow;
        MockDataStore.Assessments.Add(assessment);
        return Task.CompletedTask;
    }

    public Task AddScoreAsync(AssessmentScore score)
    {
        score.Id = MockDataStore.NextAssessmentScoreId++;
        score.CreatedAt = DateTime.UtcNow;
        MockDataStore.AssessmentScores.Add(score);
        return Task.CompletedTask;
    }

    public void Update(Assessment assessment) => assessment.UpdatedAt = DateTime.UtcNow;
    public void UpdateScore(AssessmentScore score) => score.UpdatedAt = DateTime.UtcNow;

    // ── 360° evaluator assignments ─────────────────────────────────────────

    private static AssessmentAssignment Hydrate(AssessmentAssignment a)
    {
        a.EvaluatorEmployee = MockDataStore.Employees.FirstOrDefault(e => e.Id == a.EvaluatorEmployeeId);
        var assessment = MockDataStore.Assessments.FirstOrDefault(x => x.Id == a.AssessmentId);
        if (assessment is not null)
        {
            assessment.Employee = MockDataStore.Employees.FirstOrDefault(e => e.Id == assessment.EmployeeId)!;
            assessment.Cycle    = MockDataStore.AssessmentCycles.FirstOrDefault(c => c.Id == assessment.CycleId)!;
            a.Assessment = assessment;
        }
        return a;
    }

    public Task<AssessmentAssignment?> GetAssignmentAsync(int assessmentId, int evaluatorEmployeeId)
    {
        var a = MockDataStore.AssessmentAssignments.FirstOrDefault(x =>
            x.AssessmentId == assessmentId &&
            x.EvaluatorEmployeeId == evaluatorEmployeeId &&
            !x.IsDeleted);
        return Task.FromResult(a is null ? null : Hydrate(a));
    }

    public Task AddAssignmentAsync(AssessmentAssignment assignment)
    {
        assignment.Id = MockDataStore.NextAssessmentAssignmentId++;
        assignment.CreatedAt = DateTime.UtcNow;
        MockDataStore.AssessmentAssignments.Add(assignment);
        return Task.CompletedTask;
    }

    public Task<List<AssessmentAssignment>> GetAssignmentsByEvaluatorAsync(int evaluatorEmployeeId)
    {
        var list = MockDataStore.AssessmentAssignments
            .Where(x => x.EvaluatorEmployeeId == evaluatorEmployeeId && !x.IsDeleted)
            .Select(Hydrate)
            .ToList();
        return Task.FromResult(list);
    }

    public Task<List<AssessmentAssignment>> GetAssignmentsByAssessmentAsync(int assessmentId)
    {
        var list = MockDataStore.AssessmentAssignments
            .Where(x => x.AssessmentId == assessmentId && !x.IsDeleted)
            .Select(Hydrate)
            .ToList();
        return Task.FromResult(list);
    }

    public void UpdateAssignment(AssessmentAssignment assignment) => assignment.UpdatedAt = DateTime.UtcNow;
}
