using System.Security.Claims;
using BitirmeBackend.Application.Exceptions;
using BitirmeBackend.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected int CurrentUserId =>
        int.Parse(User.FindFirstValue("userId")
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Token'da kullanıcı kimliği bulunamadı."));

    protected string CurrentUserRole =>
        User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

    protected int? CurrentEmployeeId
    {
        get
        {
            var val = User.FindFirstValue("employeeId");
            return val is not null ? int.Parse(val) : null;
        }
    }

    /// <summary>True if the given employee's ManagerId matches the manager's own employee id.</summary>
    protected async Task<bool> EmployeeBelongsToManagerAsync(int employeeId, int managerEmployeeId, IEmployeeRepository repo)
    {
        if (employeeId == managerEmployeeId) return true;
        var employee = await repo.GetByIdAsync(employeeId);
        return employee is not null && employee.ManagerId == managerEmployeeId;
    }

    /// <summary>
    /// Enforces team scoping for Manager role: throws if the employee is not on the
    /// caller's team. HR and Admin skip the check entirely.
    /// </summary>
    protected async Task EnsureManagerCanAccessEmployeeAsync(int employeeId, IEmployeeRepository repo)
    {
        if (CurrentUserRole != "Manager")
            return;

        var managerEmployeeId = CurrentEmployeeId;
        if (managerEmployeeId is null ||
            !await EmployeeBelongsToManagerAsync(employeeId, managerEmployeeId.Value, repo))
            throw new ForbiddenAccessException("Bu çalışan sizin ekibinizde bulunmamaktadır.");
    }
}
