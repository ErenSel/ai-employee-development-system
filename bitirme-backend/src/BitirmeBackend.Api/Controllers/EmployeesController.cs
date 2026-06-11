using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeBackend.Api.Controllers;

public class EmployeesController : BaseController
{
    private readonly IEmployeeService _employeeService;
    private readonly IAssessmentService _assessmentService;
    private readonly IActionPlanService _actionPlanService;
    private readonly IEmployeeRepository _employees;

    public EmployeesController(IEmployeeService employeeService, IAssessmentService assessmentService, IActionPlanService actionPlanService, IEmployeeRepository employees)
    {
        _employeeService = employeeService;
        _assessmentService = assessmentService;
        _actionPlanService = actionPlanService;
        _employees = employees;
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        if (pageNumber < 1) return BadRequest(ApiResponse<object>.Fail("pageNumber en az 1 olmalıdır."));
        if (pageSize < 1 || pageSize > 100) return BadRequest(ApiResponse<object>.Fail("pageSize 1 ile 100 arasında olmalıdır."));

        // Managers are scoped to their own team inside the query (correct paging + total)
        int? managerFilter = CurrentUserRole == "Manager" ? CurrentEmployeeId : null;
        var result = await _employeeService.GetEmployeesAsync(pageNumber, pageSize, managerFilter);
        return Ok(result);
    }

    [Authorize(Policy = "Authenticated")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var emp = await _employeeService.GetEmployeeByIdAsync(id);

        if (CurrentUserRole == "Employee")
        {
            var evaluatorEmployeeId = CurrentEmployeeId
                ?? throw new UnauthorizedAccessException("Token'da çalışan kimliği bulunamadı.");

            if (!await _assessmentService.HasActiveAssignmentForEmployeeAsync(id, evaluatorEmployeeId))
                throw new UnauthorizedAccessException("Bu çalışana erişim yetkiniz yok.");

            return Ok(ApiResponse<object>.Ok(new EmployeeBasicInfoDto
            {
                Id = emp.Id,
                FullName = emp.FullName,
                Department = emp.Department,
                JobRole = emp.JobRole
            }));
        }

        if (CurrentUserRole is not ("Admin" or "HR" or "Manager"))
            throw new UnauthorizedAccessException("Bu çalışana erişim yetkiniz yok.");

        if (CurrentUserRole == "Manager" && emp.ManagerId != CurrentEmployeeId)
            throw new UnauthorizedAccessException("Bu çalışana erişim yetkiniz yok.");

        return Ok(ApiResponse<object>.Ok(emp));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        var result = await _employeeService.CreateEmployeeAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request)
    {
        var result = await _employeeService.UpdateEmployeeAsync(id, request);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}/assessments")]
    public async Task<IActionResult> GetAssessments(int id,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) return BadRequest(ApiResponse<object>.Fail("pageNumber en az 1 olmalıdır."));
        if (pageSize < 1 || pageSize > 100) return BadRequest(ApiResponse<object>.Fail("pageSize 1 ile 100 arasında olmalıdır."));

        await EnsureManagerCanAccessEmployeeAsync(id, _employees);

        var result = await _assessmentService.GetEmployeeAssessmentsAsync(id, pageNumber, pageSize);
        return Ok(result);
    }

    [Authorize(Policy = "Authenticated")]
    [HttpGet("{id:int}/action-plans")]
    public async Task<IActionResult> GetActionPlans(int id)
    {
        if (CurrentUserRole == "Employee")
        {
            var employeeId = CurrentEmployeeId
                ?? throw new UnauthorizedAccessException("Token'da çalışan kimliği bulunamadı.");

            if (employeeId != id)
                throw new UnauthorizedAccessException("Bu çalışanın aksiyon planlarına erişim yetkiniz yok.");
        }
        else
        {
            if (CurrentUserRole is not ("Admin" or "HR" or "Manager"))
                throw new UnauthorizedAccessException("Bu çalışanın aksiyon planlarına erişim yetkiniz yok.");

            await EnsureManagerCanAccessEmployeeAsync(id, _employees);
        }

        var result = await _actionPlanService.GetEmployeeActionPlansAsync(id);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}/features")]
    public async Task<IActionResult> GetFeatures(int id, [FromQuery] int assessmentId)
    {
        await EnsureManagerCanAccessEmployeeAsync(id, _employees);

        try
        {
            var features = await _employeeService.GetEmployeeFeaturesForPredictionAsync(id, assessmentId);
            return Ok(ApiResponse<object>.Ok(features));
        }
        catch (ArgumentException ex)
        {
            var missingList = ex.Message
                .Replace("Eksik özellikler: ", string.Empty)
                .Split(", ", StringSplitOptions.RemoveEmptyEntries);

            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                missingFeatures = missingList
            });
        }
    }
}
