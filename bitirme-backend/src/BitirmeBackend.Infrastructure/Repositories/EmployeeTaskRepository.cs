using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class EmployeeTaskRepository : IEmployeeTaskRepository
{
    private readonly AppDbContext _db;
    public EmployeeTaskRepository(AppDbContext db) => _db = db;

    public async Task<(IEnumerable<EmployeeTask> Items, int TotalCount)> GetByEmployeePagedAsync(
        int employeeId, int pageNumber, int pageSize)
    {
        var query = _db.EmployeeTasks.Where(t => t.EmployeeId == employeeId && !t.IsDeleted);
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(t => t.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }

    public async Task<(IEnumerable<EmployeeTask> Items, int TotalCount)> GetByEmployeePagedWithDetailsAsync(
        int employeeId, int pageNumber, int pageSize)
    {
        var query = _db.EmployeeTasks.Where(t => t.EmployeeId == employeeId && !t.IsDeleted);
        var total = await query.CountAsync();
        var items = await query
            .Include(t => t.Employee)
            .Include(t => t.AssignedByUser)
            .Include(t => t.ActionPlanItem)
            .OrderByDescending(t => t.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }

    public Task<EmployeeTask?> GetByIdAsync(int id) =>
        _db.EmployeeTasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

    public Task<EmployeeTask?> GetByIdWithDetailsAsync(int id) =>
        _db.EmployeeTasks
            .Include(t => t.Employee)
            .Include(t => t.AssignedByUser)
            .Include(t => t.ActionPlanItem)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

    public async Task<IEnumerable<EmployeeTask>> GetByActionPlanItemIdAsync(int actionPlanItemId) =>
        await _db.EmployeeTasks
            .Where(t => t.ActionPlanItemId == actionPlanItemId && !t.IsDeleted)
            .ToListAsync();

    public async Task AddAsync(EmployeeTask task) => await _db.EmployeeTasks.AddAsync(task);

    public async Task AddRangeAsync(IEnumerable<EmployeeTask> tasks) =>
        await _db.EmployeeTasks.AddRangeAsync(tasks);

    public void Update(EmployeeTask task) => task.UpdatedAt = DateTime.UtcNow;
}
