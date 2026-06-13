using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeBackend.Api.Controllers;

[Route("api/admin")]
public class AdminController : BaseController
{
    private readonly IMaintenanceService _maintenance;

    public AdminController(IMaintenanceService maintenance)
    {
        _maintenance = maintenance;
    }

    /// <summary>
    /// DESTRUCTIVE - Admin only. Wipes all runtime-generated data (assessments, scores,
    /// assignments, AI prediction runs/actions, feedback, action plans/items, tasks/comments)
    /// so the system can be tested from a clean slate. Seed/reference tables
    /// (Users, Employees, Roles, Departments, JobRoles, Competencies, AssessmentCycles,
    /// ActionCatalogs, ModelVersions, RefreshTokens) are preserved.
    /// Requires { "confirmation": "RESET" } in the body to prevent accidental calls.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("reset-data")]
    public async Task<IActionResult> ResetTransactionalData([FromBody] ResetTransactionalDataRequest request)
    {
        if (request is null || request.Confirmation != "RESET")
            return BadRequest(ApiResponse<object>.Fail(
                "This is a destructive operation. Send \"confirmation\": \"RESET\" in the request body to confirm."));

        var counts = await _maintenance.ResetTransactionalDataAsync();

        var result = new DataResetResultDto
        {
            DeletedCounts = new Dictionary<string, int>(counts),
            TotalDeleted = counts.Values.Sum(),
            ExecutedAt = DateTime.UtcNow
        };

        return Ok(ApiResponse<object>.Ok(result, $"{result.TotalDeleted} records deleted. Reference/seed tables were preserved."));
    }
}
