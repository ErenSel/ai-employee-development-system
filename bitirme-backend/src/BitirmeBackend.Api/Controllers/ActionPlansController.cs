using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeBackend.Api.Controllers;

[Route("api/action-plans")]
public class ActionPlansController : BaseController
{
    private readonly IActionPlanService _actionPlanService;
    private readonly IPdfExportService  _pdfExportService;
    private readonly IEmployeeRepository _employees;
    private readonly IAssessmentRepository _assessments;

    public ActionPlansController(
        IActionPlanService actionPlanService,
        IPdfExportService pdfExportService,
        IEmployeeRepository employees,
        IAssessmentRepository assessments)
    {
        _actionPlanService = actionPlanService;
        _pdfExportService  = pdfExportService;
        _employees         = employees;
        _assessments       = assessments;
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateActionPlanRequest request)
    {
        // Manager team scope: resolve the target employee via the assessment
        if (CurrentUserRole == "Manager")
        {
            var assessment = await _assessments.GetByIdAsync(request.AssessmentId)
                ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {request.AssessmentId}");
            await EnsureManagerCanAccessEmployeeAsync(assessment.EmployeeId, _employees);
        }

        var result = await _actionPlanService.GenerateDraftActionPlanAsync(request, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _actionPlanService.GetActionPlanByIdAsync(id);
        await EnsureManagerCanAccessActionPlanAsync(result);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPut("{id:int}/items/{itemId:int}")]
    public async Task<IActionResult> UpdateItem(int id, int itemId, [FromBody] UpdateActionPlanItemRequest request)
    {
        await EnsureManagerCanAccessActionPlanAsync(id);
        var result = await _actionPlanService.UpdateActionPlanItemAsync(id, itemId, request);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("{id:int}/items")]
    public async Task<IActionResult> AddManualItem(int id, [FromBody] AddManualActionPlanItemRequest request)
    {
        await EnsureManagerCanAccessActionPlanAsync(id);
        var result = await _actionPlanService.AddManualItemAsync(id, request, CurrentUserId);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpDelete("{id:int}/items/{itemId:int}")]
    public async Task<IActionResult> RemoveItem(int id, int itemId)
    {
        await EnsureManagerCanAccessActionPlanAsync(id);
        await _actionPlanService.RemoveItemAsync(id, itemId);
        return NoContent();
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        await EnsureManagerCanAccessActionPlanAsync(id);
        var result = await _actionPlanService.ApproveActionPlanAsync(id, CurrentUserId);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("{id:int}/send")]
    public async Task<IActionResult> Send(int id)
    {
        await EnsureManagerCanAccessActionPlanAsync(id);
        var result = await _actionPlanService.SendActionPlanToEmployeeAsync(id, CurrentUserId);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        await EnsureManagerCanAccessActionPlanAsync(id);
        var result = await _actionPlanService.CancelActionPlanAsync(id, CurrentUserId);
        return Ok(ApiResponse<object>.Ok(result));
    }

    // Authenticated (not just HrOrManager): the plan's owning employee may also download
    // their own PDF. HR/Admin see all; Manager is scoped to their team; Employee to their own plan.
    [Authorize(Policy = "Authenticated")]
    [HttpGet("{id:int}/export-pdf")]
    public async Task<IActionResult> ExportPdf(int id)
    {
        if (CurrentUserRole == "Employee")
        {
            var plan = await _actionPlanService.GetActionPlanByIdAsync(id);
            var employeeId = CurrentEmployeeId
                ?? throw new UnauthorizedAccessException("Token'da çalışan kimliği bulunamadı.");
            if (plan.EmployeeId != employeeId)
                throw new UnauthorizedAccessException("Bu gelişim planına erişim yetkiniz yok.");
        }
        else
        {
            if (CurrentUserRole is not ("Admin" or "HR" or "Manager"))
                throw new UnauthorizedAccessException("Bu gelişim planına erişim yetkiniz yok.");
            await EnsureManagerCanAccessActionPlanAsync(id);
        }

        var bytes = await _pdfExportService.GenerateActionPlanPdfAsync(id);
        return File(bytes, "application/pdf", $"aksiyon-plani-{id}.pdf");
    }

    private async Task EnsureManagerCanAccessActionPlanAsync(int actionPlanId)
    {
        if (CurrentUserRole != "Manager")
            return;

        var plan = await _actionPlanService.GetActionPlanByIdAsync(actionPlanId);
        await EnsureManagerCanAccessActionPlanAsync(plan);
    }

    private async Task EnsureManagerCanAccessActionPlanAsync(ActionPlanDetailDto plan)
    {
        await EnsureManagerCanAccessEmployeeAsync(plan.EmployeeId, _employees);
    }
}
