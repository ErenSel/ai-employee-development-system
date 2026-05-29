using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;

namespace BitirmeBackend.Infrastructure.MockRepositories;

public class MockEmployeeRepository : IEmployeeRepository
{
    private static IEnumerable<Employee> Active =>
        MockDataStore.Employees.Where(e => !e.IsDeleted);

    public Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var all = Active.ToList();
        var items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return Task.FromResult((items, all.Count));
    }

    public Task<Employee?> GetByIdAsync(int id) =>
        Task.FromResult(Active.FirstOrDefault(e => e.Id == id));

    public Task<Employee?> GetByIdWithDetailsAsync(int id)
    {
        var emp = Active.FirstOrDefault(e => e.Id == id);
        if (emp is not null)
        {
            emp.Department = MockDataStore.Departments.FirstOrDefault(d => d.Id == emp.DepartmentId)!;
            emp.JobRole    = MockDataStore.JobRoles.FirstOrDefault(j => j.Id == emp.JobRoleId)!;
            emp.Manager    = emp.ManagerId.HasValue
                ? Active.FirstOrDefault(e => e.Id == emp.ManagerId.Value)
                : null;
        }
        return Task.FromResult(emp);
    }

    public Task<Employee?> GetByCodeAsync(string employeeCode) =>
        Task.FromResult(Active.FirstOrDefault(e =>
            e.EmployeeCode.Equals(employeeCode, StringComparison.OrdinalIgnoreCase)));

    public Task<bool> ExistsByCodeAsync(string employeeCode) =>
        Task.FromResult(Active.Any(e =>
            e.EmployeeCode.Equals(employeeCode, StringComparison.OrdinalIgnoreCase)));

    public Task AddAsync(Employee employee)
    {
        employee.Id = MockDataStore.NextEmployeeId++;
        employee.CreatedAt = DateTime.UtcNow;
        MockDataStore.Employees.Add(employee);
        return Task.CompletedTask;
    }

    public void Update(Employee employee) => employee.UpdatedAt = DateTime.UtcNow;
}
