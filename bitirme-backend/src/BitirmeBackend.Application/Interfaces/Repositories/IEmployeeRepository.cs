using BitirmeBackend.Domain.Entities;

namespace BitirmeBackend.Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    /// <summary>
    /// Paged employee list. When <paramref name="managerId"/> is provided, only employees
    /// whose ManagerId matches are returned (filtered inside the query, before paging).
    /// </summary>
    Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, int? managerId = null);
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByIdWithDetailsAsync(int id);
    Task<Employee?> GetByCodeAsync(string employeeCode);
    Task<bool> ExistsByCodeAsync(string employeeCode);
    Task AddAsync(Employee employee);
    void Update(Employee employee);
    Task<bool> DepartmentExistsAsync(int departmentId);
    Task<bool> JobRoleExistsAsync(int jobRoleId);
}
