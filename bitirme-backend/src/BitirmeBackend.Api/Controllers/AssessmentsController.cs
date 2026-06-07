using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
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
        var result = await _assessmentService.CompleteAssessmentAsync(id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}/scores")]
    public async Task<IActionResult> GetScores(int id)
    {
        var scores = await _assessmentService.GetAssessmentScoresAsync(id);
        return Ok(ApiResponse<object>.Ok(scores));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("{id:int}/scores")]
    public async Task<IActionResult> UpsertScore(int id, [FromBody] UpsertAssessmentScoreRequest request)
    {
        var result = await _assessmentService.UpsertAssessmentScoreAsync(id, request);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPut("{id:int}/scores/{scoreId:int}")]
    public async Task<IActionResult> UpdateScore(int id, int scoreId, [FromBody] UpsertAssessmentScoreRequest request)
    {
        // Validate range before calling service (scoreId is used only for routing; service
        // updates by assessmentId + competencyId + evaluatorEmployeeId)
        if (request.Score < 0.0 || request.Score > 5.0)
            return BadRequest(ApiResponse<object>.Fail($"Skor 0 ile 5 arasında olmalıdır. Girilen değer: {request.Score}"));

        var result = await _assessmentService.UpsertAssessmentScoreAsync(id, request);
        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>Submits an evaluator's full set of competency scores in one request.</summary>
    [Authorize(Policy = "Authenticated")]
    [HttpPost("{id:int}/scores/bulk")]
    public async Task<IActionResult> BulkUpsertScores(int id, [FromBody] BulkUpsertAssessmentScoreRequest request)
    {
        var result = await _assessmentService.BulkUpsertScoresAsync(id, request);
        return Ok(ApiResponse<object>.Ok(result));
    }

    // ── 360° evaluator assignments ─────────────────────────────────────────

    [Authorize(Policy = "HrOrManager")]
    [HttpPost("{id:int}/assignments")]
    public async Task<IActionResult> CreateAssignment(int id, [FromBody] CreateAssessmentAssignmentRequest request)
    {
        // Route id is authoritative for the assessment
        request.AssessmentId = id;
        var result = await _assessmentService.CreateAssignmentAsync(request, CurrentUserId);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}/assignments")]
    public async Task<IActionResult> GetAssignments(int id)
    {
        var result = await _assessmentService.GetAssignmentsByAssessmentAsync(id);
        return Ok(ApiResponse<object>.Ok(result));
    }
}
