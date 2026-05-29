using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeBackend.Api.Controllers;

public class EmployeesController : BaseController
{
    private readonly IEmployeeService _employeeService;
    private readonly IAssessmentService _assessmentService;

    public EmployeesController(IEmployeeService employeeService, IAssessmentService assessmentService)
    {
        _employeeService = employeeService;
        _assessmentService = assessmentService;
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _employeeService.GetEmployeesAsync(pageNumber, pageSize);

        if (CurrentUserRole == "Manager")
        {
            var myEmpId = CurrentEmployeeId;
            var filtered = result.Data.Where(e => e.ManagerId == myEmpId).ToList();
            return Ok(PagedResponse<object>.Ok(filtered.Cast<object>(), filtered.Count, pageNumber, pageSize));
        }

        return Ok(result);
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var emp = await _employeeService.GetEmployeeByIdAsync(id);

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
        var result = await _assessmentService.GetEmployeeAssessmentsAsync(id, pageNumber, pageSize);
        return Ok(result);
    }

    [Authorize(Policy = "HrOrManager")]
    [HttpGet("{id:int}/features")]
    public async Task<IActionResult> GetFeatures(int id, [FromQuery] int assessmentId)
    {
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
