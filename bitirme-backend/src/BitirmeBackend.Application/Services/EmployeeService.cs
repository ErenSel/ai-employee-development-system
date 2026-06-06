using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Contracts.Responses;
using BitirmeBackend.Domain.Entities;

namespace BitirmeBackend.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employees;
    private readonly IAssessmentRepository _assessments;

    public EmployeeService(IEmployeeRepository employees, IAssessmentRepository assessments)
    {
        _employees = employees;
        _assessments = assessments;
    }

    public async Task<PagedResponse<EmployeeListItemDto>> GetEmployeesAsync(int pageNumber, int pageSize, int? managerId = null)
    {
        var (items, total) = await _employees.GetPagedAsync(pageNumber, pageSize, managerId);
        var dtos = items.Select(ToListItem);
        return PagedResponse<EmployeeListItemDto>.Ok(dtos, total, pageNumber, pageSize);
    }

    public async Task<EmployeeDetailDto> GetEmployeeByIdAsync(int id)
    {
        var emp = await _employees.GetByIdWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"Çalışan bulunamadı: {id}");
        return ToDetail(emp);
    }

    public async Task<EmployeeDetailDto> CreateEmployeeAsync(CreateEmployeeRequest request)
    {
        var emp = new Employee
        {
            EmployeeCode          = request.EmployeeCode,
            FullName              = request.FullName,
            Email                 = request.Email,
            Age                   = request.Age,
            Gender                = request.Gender,
            DepartmentId          = request.DepartmentId,
            JobRoleId             = request.JobRoleId,
            ManagerId             = request.ManagerId,
            Education             = request.Education,
            EducationField        = request.EducationField,
            BusinessTravel        = request.BusinessTravel,
            MaritalStatus         = request.MaritalStatus,
            DistanceFromHome      = request.DistanceFromHome,
            EnvironmentSatisfaction = request.EnvironmentSatisfaction,
            JobSatisfaction       = request.JobSatisfaction,
            WorkLifeBalance       = request.WorkLifeBalance,
            TotalWorkingYears     = request.TotalWorkingYears,
            YearsAtCompany        = request.YearsAtCompany,
            YearsInCurrentRole    = request.YearsInCurrentRole,
            YearsWithCurrManager  = request.YearsWithCurrManager,
            PerformanceScore      = request.PerformanceScore,
            Attrition             = request.Attrition,
            IsActive              = true
        };

        await _employees.AddAsync(emp);
        var created = await _employees.GetByIdWithDetailsAsync(emp.Id)
            ?? throw new InvalidOperationException("Oluşturulan çalışan yüklenemedi.");
        return ToDetail(created);
    }

    public async Task<EmployeeDetailDto> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request)
    {
        var emp = await _employees.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Çalışan bulunamadı: {id}");

        emp.FullName              = request.FullName;
        emp.Email                 = request.Email;
        emp.Age                   = request.Age;
        emp.Gender                = request.Gender;
        emp.DepartmentId          = request.DepartmentId;
        emp.JobRoleId             = request.JobRoleId;
        emp.ManagerId             = request.ManagerId;
        emp.Education             = request.Education;
        emp.EducationField        = request.EducationField;
        emp.BusinessTravel        = request.BusinessTravel;
        emp.MaritalStatus         = request.MaritalStatus;
        emp.DistanceFromHome      = request.DistanceFromHome;
        emp.EnvironmentSatisfaction = request.EnvironmentSatisfaction;
        emp.JobSatisfaction       = request.JobSatisfaction;
        emp.WorkLifeBalance       = request.WorkLifeBalance;
        emp.TotalWorkingYears     = request.TotalWorkingYears;
        emp.YearsAtCompany        = request.YearsAtCompany;
        emp.YearsInCurrentRole    = request.YearsInCurrentRole;
        emp.YearsWithCurrManager  = request.YearsWithCurrManager;
        emp.PerformanceScore      = request.PerformanceScore;
        emp.Attrition             = request.Attrition;
        emp.IsActive              = request.IsActive;
        emp.UpdatedAt             = DateTime.UtcNow;

        _employees.Update(emp);

        var updated = await _employees.GetByIdWithDetailsAsync(id)
            ?? throw new InvalidOperationException("Güncellenen çalışan yüklenemedi.");
        return ToDetail(updated);
    }

    public async Task<EmployeeFeatureDto> GetEmployeeFeaturesForPredictionAsync(int employeeId, int assessmentId)
    {
        var emp = await _employees.GetByIdWithDetailsAsync(employeeId)
            ?? throw new KeyNotFoundException($"Çalışan bulunamadı: {employeeId}");

        var assessment = await _assessments.GetByIdAsync(assessmentId)
            ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {assessmentId}");

        if (assessment.EmployeeId != employeeId)
            throw new ArgumentException("Değerlendirme bu çalışana ait değil.");

        var scores = (await _assessments.GetScoresByAssessmentIdAsync(assessmentId)).ToList();

        // Group every evaluator's score by competency code for 360° consolidation.
        var scoresByCode = scores
            .Where(s => s.Competency is not null)
            .GroupBy(s => s.Competency.Code)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Consolidation formula: average within each evaluator category (Self/Manager/
        // Peer/Subordinate), then average across the categories that are present.
        // e.g. Self + Manager + 2 Peers → (Self + Manager + Avg(Peers)) / 3.
        double Consolidate(string code) =>
            scoresByCode[code]
                .GroupBy(s => s.EvaluatorType)
                .Select(g => g.Average(x => x.Score))
                .Average();

        // Validate required competency codes
        string[] requiredCodes =
        [
            "Core_Communication", "Core_Teamwork", "Core_ProblemSolving", "Core_Adaptability",
            "Core_Initiative", "Core_Accountability", "Core_LearningAgility", "Core_TimeManagement",
            "Dept_Comp1", "Dept_Comp2", "Dept_Comp3", "Role_Comp1", "Role_Comp2"
        ];

        var missing = new List<string>();

        // Employee field checks
        if (string.IsNullOrWhiteSpace(emp.Attrition))          missing.Add("Attrition");
        if (string.IsNullOrWhiteSpace(emp.BusinessTravel))     missing.Add("BusinessTravel");
        if (emp.Department is null)                             missing.Add("Department");
        if (string.IsNullOrWhiteSpace(emp.Education))          missing.Add("Education");
        if (string.IsNullOrWhiteSpace(emp.EducationField))     missing.Add("EducationField");
        if (string.IsNullOrWhiteSpace(emp.Gender))             missing.Add("Gender");
        if (emp.JobRole is null)                                missing.Add("JobRole");
        if (string.IsNullOrWhiteSpace(emp.MaritalStatus))      missing.Add("MaritalStatus");

        foreach (var code in requiredCodes)
        {
            if (!scoresByCode.ContainsKey(code))
                missing.Add(code);
        }

        if (missing.Count > 0)
            throw new ArgumentException($"Eksik özellikler: {string.Join(", ", missing)}");

        return new EmployeeFeatureDto
        {
            Age                    = emp.Age,
            Attrition              = emp.Attrition,
            BusinessTravel         = emp.BusinessTravel,
            Department             = emp.Department?.Name ?? string.Empty,
            DistanceFromHome       = emp.DistanceFromHome,
            Education              = emp.Education,
            EducationField         = emp.EducationField,
            EnvironmentSatisfaction = emp.EnvironmentSatisfaction,
            Gender                 = emp.Gender,
            JobRole                = emp.JobRole?.Name ?? string.Empty,
            JobSatisfaction        = emp.JobSatisfaction,
            MaritalStatus          = emp.MaritalStatus,
            WorkLifeBalance        = emp.WorkLifeBalance,
            TotalWorkingYears      = emp.TotalWorkingYears,
            YearsAtCompany         = emp.YearsAtCompany,
            YearsInCurrentRole     = emp.YearsInCurrentRole,
            YearsWithCurrManager   = emp.YearsWithCurrManager,
            // PerformanceScore sent to ML prefers the assessment's OverallScore;
            // Employee.PerformanceScore is the cached latest/general score fallback.
            PerformanceScore       = assessment.OverallScore ?? emp.PerformanceScore,
            Core_Communication     = Consolidate("Core_Communication"),
            Core_Teamwork          = Consolidate("Core_Teamwork"),
            Core_ProblemSolving    = Consolidate("Core_ProblemSolving"),
            Core_Adaptability      = Consolidate("Core_Adaptability"),
            Core_Initiative        = Consolidate("Core_Initiative"),
            Core_Accountability    = Consolidate("Core_Accountability"),
            Core_LearningAgility   = Consolidate("Core_LearningAgility"),
            Core_TimeManagement    = Consolidate("Core_TimeManagement"),
            Dept_Comp1             = Consolidate("Dept_Comp1"),
            Dept_Comp2             = Consolidate("Dept_Comp2"),
            Dept_Comp3             = Consolidate("Dept_Comp3"),
            Role_Comp1             = Consolidate("Role_Comp1"),
            Role_Comp2             = Consolidate("Role_Comp2"),
        };
    }

    // ── Mapping helpers ──────────────────────────────────────────────────────

    private static EmployeeListItemDto ToListItem(Employee e) => new()
    {
        Id           = e.Id,
        EmployeeCode = e.EmployeeCode,
        FullName     = e.FullName,
        Email        = e.Email,
        Department   = e.Department?.Name ?? string.Empty,
        JobRole      = e.JobRole?.Name ?? string.Empty,
        ManagerId    = e.ManagerId,
        IsActive     = e.IsActive
    };

    private static EmployeeDetailDto ToDetail(Employee e) => new()
    {
        Id                      = e.Id,
        EmployeeCode            = e.EmployeeCode,
        FullName                = e.FullName,
        Email                   = e.Email,
        Age                     = e.Age,
        Gender                  = e.Gender,
        DepartmentId            = e.DepartmentId,
        Department              = e.Department?.Name ?? string.Empty,
        JobRoleId               = e.JobRoleId,
        JobRole                 = e.JobRole?.Name ?? string.Empty,
        ManagerId               = e.ManagerId,
        ManagerName             = e.Manager?.FullName,
        Education               = e.Education,
        EducationField          = e.EducationField,
        BusinessTravel          = e.BusinessTravel,
        MaritalStatus           = e.MaritalStatus,
        DistanceFromHome        = e.DistanceFromHome,
        EnvironmentSatisfaction = e.EnvironmentSatisfaction,
        JobSatisfaction         = e.JobSatisfaction,
        WorkLifeBalance         = e.WorkLifeBalance,
        TotalWorkingYears       = e.TotalWorkingYears,
        YearsAtCompany          = e.YearsAtCompany,
        YearsInCurrentRole      = e.YearsInCurrentRole,
        YearsWithCurrManager    = e.YearsWithCurrManager,
        PerformanceScore        = e.PerformanceScore,
        Attrition               = e.Attrition,
        IsActive                = e.IsActive
    };
}
