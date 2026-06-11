using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Domain.Enums;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class ActionPlanRepository : IActionPlanRepository
{
    private readonly AppDbContext _db;
    public ActionPlanRepository(AppDbContext db) => _db = db;

    public Task<ActionPlan?> GetByIdAsync(int id) =>
        _db.ActionPlans.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

    public Task<ActionPlan?> GetByIdWithItemsAsync(int id) =>
        _db.ActionPlans
            .Include(p => p.Employee)
            .Include(p => p.CreatedByUser)
            .Include(p => p.Items.Where(i => !i.IsDeleted))
                .ThenInclude(i => i.EmployeeTasks.Where(t => !t.IsDeleted))
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

    public async Task<IEnumerable<ActionPlan>> GetByEmployeeIdAsync(int employeeId) =>
        await _db.ActionPlans
            .Where(p => p.EmployeeId == employeeId && !p.IsDeleted)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

    public async Task<IEnumerable<ActionPlan>> GetByEmployeeIdWithItemsAsync(int employeeId) =>
        await _db.ActionPlans
            .Include(p => p.Employee)
            .Include(p => p.CreatedByUser)
            .Include(p => p.Items.Where(i => !i.IsDeleted))
                .ThenInclude(i => i.EmployeeTasks.Where(t => !t.IsDeleted))
            .Where(p => p.EmployeeId == employeeId && !p.IsDeleted)
            .OrderByDescending(p => p.Id)
            .AsSplitQuery()
            .ToListAsync();

    public Task<ActionPlan?> GetActiveByAssessmentIdAsync(int assessmentId) =>
        _db.ActionPlans.FirstOrDefaultAsync(p =>
            p.AssessmentId == assessmentId &&
            !p.IsDeleted &&
            p.Status != ActionPlanStatus.Cancelled &&
            p.Status != ActionPlanStatus.Completed);

    public Task<ActionPlanItem?> GetItemByIdAsync(int itemId) =>
        _db.ActionPlanItems.FirstOrDefaultAsync(i => i.Id == itemId && !i.IsDeleted);

    public async Task AddAsync(ActionPlan plan) => await _db.ActionPlans.AddAsync(plan);

    public async Task AddItemAsync(ActionPlanItem item) => await _db.ActionPlanItems.AddAsync(item);

    public void Update(ActionPlan plan) => plan.UpdatedAt = DateTime.UtcNow;

    public void UpdateItem(ActionPlanItem item) => item.UpdatedAt = DateTime.UtcNow;

    public void RemoveItem(ActionPlanItem item)
    {
        item.IsDeleted = true;
        item.UpdatedAt = DateTime.UtcNow;
    }
}
