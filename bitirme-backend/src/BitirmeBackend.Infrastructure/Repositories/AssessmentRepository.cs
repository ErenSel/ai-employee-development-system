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

    public async Task AddAsync(Assessment assessment) => await _db.Assessments.AddAsync(assessment);

    public async Task AddScoreAsync(AssessmentScore score) => await _db.AssessmentScores.AddAsync(score);

    public void Update(Assessment assessment) => assessment.UpdatedAt = DateTime.UtcNow;

    public void UpdateScore(AssessmentScore score) => score.UpdatedAt = DateTime.UtcNow;
}
