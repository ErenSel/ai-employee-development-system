using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeBackend.Api.Controllers;

public class AssessmentsController : BaseController
{
    private readonly IAssessmentService _assessmentService;
    private readonly IEmployeeRepository _employees;

    public AssessmentsController(IAssessmentService assessmentService, IEmployeeRepository employees)
    {
        _assessmentService = assessmentService;
        _employees = employees;
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _assessmentService.GetAssessmentByIdAsync(id);
        await EnsureManagerCanAccessAssessmentAsync(result);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssessmentRequest request)
    {
        await EnsureManagerCanAccessEmployeeAsync(request.EmployeeId, _employees);

        var result = await _assessmentService.CreateAssessmentAsync(request, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPut("{id:int}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        await EnsureManagerCanAccessAssessmentAsync(id);
        var result = await _assessmentService.CompleteAssessmentAsync(id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "Authenticated")]
    [HttpGet("{id:int}/scores")]
    public async Task<IActionResult> GetScores(int id)
    {
        if (CurrentUserRole == "Employee")
        {
            var evaluatorEmployeeId = CurrentEmployeeId
                ?? throw new UnauthorizedAccessException("Token'da çalışan kimliği bulunamadı.");

            if (!await _assessmentService.HasAssignmentAsync(id, evaluatorEmployeeId))
                throw new UnauthorizedAccessException("Bu değerlendirme skorlarına erişim yetkiniz yok.");

            var evaluatorScores = await _assessmentService.GetAssessmentScoresForEvaluatorAsync(id, evaluatorEmployeeId);
            return Ok(ApiResponse<object>.Ok(evaluatorScores));
        }

        if (CurrentUserRole is not ("Admin" or "HR" or "Manager"))
            throw new UnauthorizedAccessException("Bu değerlendirme skorlarına erişim yetkiniz yok.");

        await EnsureManagerCanAccessAssessmentAsync(id);
        var scores = await _assessmentService.GetAssessmentScoresAsync(id);
        return Ok(ApiResponse<object>.Ok(scores));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("{id:int}/scores")]
    public async Task<IActionResult> UpsertScore(int id, [FromBody] UpsertAssessmentScoreRequest request)
    {
        await EnsureManagerCanAccessAssessmentAsync(id);
        var result = await _assessmentService.UpsertAssessmentScoreAsync(id, request, IsProxyAllowed);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPut("{id:int}/scores/{scoreId:int}")]
    public async Task<IActionResult> UpdateScore(int id, int scoreId, [FromBody] UpsertAssessmentScoreRequest request)
    {
        await EnsureManagerCanAccessAssessmentAsync(id);

        // Validate range before calling service (scoreId is used only for routing; service
        // updates by assessmentId + competencyId + evaluatorEmployeeId)
        if (request.Score < 0.0 || request.Score > 5.0)
            return BadRequest(ApiResponse<object>.Fail($"Skor 0 ile 5 arasında olmalıdır. Girilen değer: {request.Score}"));

        var result = await _assessmentService.UpsertAssessmentScoreAsync(id, request, IsProxyAllowed);
        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>Submits an evaluator's full set of competency scores in one request.</summary>
    [Authorize(Policy = "Authenticated")]
    [HttpPost("{id:int}/scores/bulk")]
    public async Task<IActionResult> BulkUpsertScores(int id, [FromBody] BulkUpsertAssessmentScoreRequest request)
    {
        var result = await _assessmentService.BulkUpsertScoresAsync(id, request, IsProxyAllowed);
        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>
    /// HR and Admin may enter scores on anyone's behalf ("Temsili / God Mode"), bypassing the
    /// 360° assignment gate. Manager and Employee remain bound to their own assignments.
    /// </summary>
    private bool IsProxyAllowed => CurrentUserRole is "HR" or "Admin";

    // ── 360° evaluator assignments ─────────────────────────────────────────

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("{id:int}/assignments")]
    public async Task<IActionResult> CreateAssignment(int id, [FromBody] CreateAssessmentAssignmentRequest request)
    {
        await EnsureManagerCanAccessAssessmentAsync(id);

        // Route id is authoritative for the assessment
        request.AssessmentId = id;
        var result = await _assessmentService.CreateAssignmentAsync(request, CurrentUserId);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}/assignments")]
    public async Task<IActionResult> GetAssignments(int id)
    {
        await EnsureManagerCanAccessAssessmentAsync(id);
        var result = await _assessmentService.GetAssignmentsByAssessmentAsync(id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    private async Task EnsureManagerCanAccessAssessmentAsync(int assessmentId)
    {
        if (CurrentUserRole != "Manager")
            return;

        var assessment = await _assessmentService.GetAssessmentByIdAsync(assessmentId);
        await EnsureManagerCanAccessAssessmentAsync(assessment);
    }

    private async Task EnsureManagerCanAccessAssessmentAsync(AssessmentDetailDto assessment)
    {
        await EnsureManagerCanAccessEmployeeAsync(assessment.EmployeeId, _employees);
    }
}
