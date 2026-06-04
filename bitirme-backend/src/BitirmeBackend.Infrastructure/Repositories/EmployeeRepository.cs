using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _db;
    public EmployeeRepository(AppDbContext db) => _db = db;

    public async Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _db.Employees.Where(e => !e.IsDeleted);
        var total = await query.CountAsync();
        var items = await query
            .OrderBy(e => e.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(e => e.Department)
            .Include(e => e.JobRole)
            .ToListAsync();
        return (items, total);
    }

    public Task<Employee?> GetByIdAsync(int id) =>
        _db.Employees.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

    public Task<Employee?> GetByIdWithDetailsAsync(int id) =>
        _db.Employees
            .Include(e => e.Department)
            .Include(e => e.JobRole)
            .Include(e => e.Manager)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

    public Task<Employee?> GetByCodeAsync(string employeeCode) =>
        _db.Employees.FirstOrDefaultAsync(e =>
            e.EmployeeCode.ToLower() == employeeCode.ToLower() && !e.IsDeleted);

    public Task<bool> ExistsByCodeAsync(string employeeCode) =>
        _db.Employees.AnyAsync(e =>
            e.EmployeeCode.ToLower() == employeeCode.ToLower() && !e.IsDeleted);

    public async Task AddAsync(Employee employee) => await _db.Employees.AddAsync(employee);

    public void Update(Employee employee) => employee.UpdatedAt = DateTime.UtcNow;
}
