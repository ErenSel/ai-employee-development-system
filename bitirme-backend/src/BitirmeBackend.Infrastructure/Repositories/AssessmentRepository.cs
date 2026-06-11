using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Domain.Enums;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class AssessmentRepository : IAssessmentRepository
{
    private readonly AppDbContext _db;
    public AssessmentRepository(AppDbContext db) => _db = db;

    public Task<Assessment?> GetByIdAsync(int id) =>
        _db.Assessments.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

    public Task<Assessment?> GetByIdWithDetailsAsync(int id) =>
        _db.Assessments
            .Include(a => a.Employee)
            .Include(a => a.Cycle)
            .Include(a => a.CreatedByUser)
            .Include(a => a.Scores.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.Competency)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

    public async Task<(IEnumerable<Assessment> Items, int TotalCount)> GetByEmployeePagedAsync(
        int employeeId, int pageNumber, int pageSize)
    {
        var query = _db.Assessments.Where(a => a.EmployeeId == employeeId && !a.IsDeleted);
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(a => a.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }

    public async Task<(IEnumerable<Assessment> Items, int TotalCount)> GetByEmployeePagedWithDetailsAsync(
        int employeeId, int pageNumber, int pageSize)
    {
        var query = _db.Assessments.Where(a => a.EmployeeId == employeeId && !a.IsDeleted);
        var total = await query.CountAsync();
        var items = await query
            .Include(a => a.Employee)
            .Include(a => a.Cycle)
            .Include(a => a.CreatedByUser)
            .Include(a => a.Scores.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.Competency)
            .OrderByDescending(a => a.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsSplitQuery()
            .ToListAsync();
        return (items, total);
    }

    public async Task<IEnumerable<AssessmentScore>> GetScoresByAssessmentIdAsync(int assessmentId) =>
        await _db.AssessmentScores
            .Include(s => s.Competency)
            .Where(s => s.AssessmentId == assessmentId && !s.IsDeleted)
            .ToListAsync();

    public Task<AssessmentScore?> GetScoreAsync(int assessmentId, int competencyId, string evaluatorType)
    {
        if (!Enum.TryParse<EvaluatorType>(evaluatorType, ignoreCase: true, out var et))
            return Task.FromResult<AssessmentScore?>(null);

        return _db.AssessmentScores.FirstOrDefaultAsync(s =>
            s.AssessmentId == assessmentId &&
            s.CompetencyId == competencyId &&
            s.EvaluatorType == et &&
            !s.IsDeleted);
    }

    public Task<AssessmentScore?> GetScoreAsync(int assessmentId, int competencyId, int evaluatorEmployeeId) =>
        _db.AssessmentScores.FirstOrDefaultAsync(s =>
            s.AssessmentId == assessmentId &&
            s.CompetencyId == competencyId &&
            s.EvaluatorEmployeeId == evaluatorEmployeeId &&
            !s.IsDeleted);

    public async Task AddAsync(Assessment assessment) => await _db.Assessments.AddAsync(assessment);

    public async Task AddScoreAsync(AssessmentScore score) => await _db.AssessmentScores.AddAsync(score);

    public void Update(Assessment assessment) => assessment.UpdatedAt = DateTime.UtcNow;

    public void UpdateScore(AssessmentScore score) => score.UpdatedAt = DateTime.UtcNow;

    // ── 360° evaluator assignments ─────────────────────────────────────────

    public Task<AssessmentAssignment?> GetAssignmentAsync(int assessmentId, int evaluatorEmployeeId) =>
        _db.AssessmentAssignments
            .Include(a => a.EvaluatorEmployee)
            .FirstOrDefaultAsync(a =>
                a.AssessmentId == assessmentId &&
                a.EvaluatorEmployeeId == evaluatorEmployeeId &&
                !a.IsDeleted);

    public async Task AddAssignmentAsync(AssessmentAssignment assignment) =>
        await _db.AssessmentAssignments.AddAsync(assignment);

    public async Task<List<AssessmentAssignment>> GetAssignmentsByEvaluatorAsync(int evaluatorEmployeeId) =>
        await _db.AssessmentAssignments
            .Include(a => a.EvaluatorEmployee)
            .Include(a => a.Assessment).ThenInclude(ass => ass.Employee)
            .Include(a => a.Assessment).ThenInclude(ass => ass.Cycle)
            .Where(a => a.EvaluatorEmployeeId == evaluatorEmployeeId && !a.IsDeleted)
            .ToListAsync();

    public async Task<List<AssessmentAssignment>> GetAssignmentsByAssessmentAsync(int assessmentId) =>
        await _db.AssessmentAssignments
            .Include(a => a.EvaluatorEmployee)
            .Where(a => a.AssessmentId == assessmentId && !a.IsDeleted)
            .ToListAsync();

    public void UpdateAssignment(AssessmentAssignment assignment) => assignment.UpdatedAt = DateTime.UtcNow;
}
