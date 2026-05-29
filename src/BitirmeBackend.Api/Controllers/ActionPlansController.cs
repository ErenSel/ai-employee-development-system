using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeBackend.Api.Controllers;

public class ActionPlansController : BaseController
{
    private readonly IActionPlanService _actionPlanService;

    public ActionPlansController(IActionPlanService actionPlanService) =>
        _actionPlanService = actionPlanService;

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateActionPlanRequest request)
    {
        var result = await _actionPlanService.GenerateDraftActionPlanAsync(request, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _actionPlanService.GetActionPlanByIdAsync(id);
        return Ok(ApiResponse<object>.Ok(result));
    }
}
