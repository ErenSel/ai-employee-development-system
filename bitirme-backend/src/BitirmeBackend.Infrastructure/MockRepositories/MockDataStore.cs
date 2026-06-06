using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Domain.Enums;
using BitirmeBackend.Infrastructure.Persistence.Seed;

namespace BitirmeBackend.Infrastructure.MockRepositories;

/// <summary>
/// Centralised in-memory seed data shared across all mock repositories.
/// Passwords stored as BCrypt hashes — never plain text.
/// </summary>
internal static class MockDataStore
{
    // ── Roles ─────────────────────────────────────────────────────────────
    public static List<Role> Roles { get; } =
    [
        new Role { Id = 1, Name = "Admin",    Description = "Full system access",          CreatedAt = DateTime.UtcNow },
        new Role { Id = 2, Name = "HR",       Description = "Human resources management",  CreatedAt = DateTime.UtcNow },
        new Role { Id = 3, Name = "Manager",  Description = "Team management",             CreatedAt = DateTime.UtcNow },
        new Role { Id = 4, Name = "Employee", Description = "Standard employee access",    CreatedAt = DateTime.UtcNow },
    ];

    // ── Departments (referenced by employees) ─────────────────────────────
    public static List<Department> Departments { get; } =
    [
        new Department { Id = 1, Name = "Sales",                Code = "SALES", CreatedAt = DateTime.UtcNow },
        new Department { Id = 2, Name = "Information Technology", Code = "IT",    CreatedAt = DateTime.UtcNow },
        new Department { Id = 3, Name = "Human Resources",       Code = "HR",    CreatedAt = DateTime.UtcNow },
    ];

    // ── Job Roles ─────────────────────────────────────────────────────────
    public static List<JobRole> JobRoles { get; } =
    [
        new JobRole { Id = 1, Name = "Sales Executive", DepartmentId = 1, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 2, Name = "Sales Manager",   DepartmentId = 1, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 3, Name = "Software Engineer", DepartmentId = 2, CreatedAt = DateTime.UtcNow },
    ];

    // ── Employees ─────────────────────────────────────────────────────────
    public static List<Employee> Employees { get; } =
    [
        new Employee
        {
            Id = 1, EmployeeCode = "EMP001", FullName = "Ayşe Kaya",
            Email = "employee@demo.com", Age = 32, Gender = "Female",
            DepartmentId = 1, JobRoleId = 1, ManagerId = 2,
            Education = "3", EducationField = "Life Sciences",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Single",
            DistanceFromHome = 5, EnvironmentSatisfaction = 3, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 8, YearsAtCompany = 5,
            YearsInCurrentRole = 3, YearsWithCurrManager = 2,
            PerformanceScore = 3.5, Attrition = "No", IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 2, EmployeeCode = "MGR001", FullName = "Mehmet Yılmaz",
            Email = "manager@demo.com", Age = 40, Gender = "Male",
            DepartmentId = 1, JobRoleId = 2, ManagerId = null,
            Education = "4", EducationField = "Business",
            BusinessTravel = "Travel_Frequently", MaritalStatus = "Married",
            DistanceFromHome = 10, EnvironmentSatisfaction = 4, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 15, YearsAtCompany = 8,
            YearsInCurrentRole = 5, YearsWithCurrManager = 3,
            PerformanceScore = 4.2, Attrition = "No", IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 3, EmployeeCode = "EMP002", FullName = "Ali Demir",
            Email = "ali.demir@demo.com", Age = 28, Gender = "Male",
            DepartmentId = 2, JobRoleId = 3, ManagerId = null,
            Education = "4", EducationField = "Computer Science",
            BusinessTravel = "Non-Travel", MaritalStatus = "Single",
            DistanceFromHome = 3, EnvironmentSatisfaction = 4, JobSatisfaction = 3,
            WorkLifeBalance = 4, TotalWorkingYears = 5, YearsAtCompany = 3,
            YearsInCurrentRole = 2, YearsWithCurrManager = 2,
            PerformanceScore = 3.8, Attrition = "No", IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 4, EmployeeCode = "EMP003", FullName = "Zeynep Arslan",
            Email = "zeynep.arslan@demo.com", Age = 30, Gender = "Female",
            DepartmentId = 1, JobRoleId = 1, ManagerId = 2,
            Education = "3", EducationField = "Marketing",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Married",
            DistanceFromHome = 7, EnvironmentSatisfaction = 3, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 7, YearsAtCompany = 4,
            YearsInCurrentRole = 2, YearsWithCurrManager = 2,
            PerformanceScore = 3.6, Attrition = "No", IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
    ];

    // ── Users (passwords as BCrypt hashes) ───────────────────────────────
    public static List<User> Users { get; } =
    [
        new User
        {
            Id = 1, FullName = "Admin User", Email = "admin@demo.com",
            PasswordHash = "$2a$11$zfRkFzsfjJ/dMsTdAhq.ie1gKjkUH6hRdzo.8yAZTQLLWgpwhQ4QC", // Admin1234!
            RoleId = 1, EmployeeId = null, IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new User
        {
            Id = 2, FullName = "HR User", Email = "hr@demo.com",
            PasswordHash = "$2a$11$nCoV4NjJ9bj4oP5K3riM2e4gNOPO0Kk3rKmwWZI2ne6gXrnPM.Rym", // Hr1234!
            RoleId = 2, EmployeeId = null, IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new User
        {
            Id = 3, FullName = "Mehmet Yılmaz", Email = "manager@demo.com",
            PasswordHash = "$2a$11$/EjKk6BgJtPAZjDW3H9zmeHaUMH.nbtF11lhEvhE7dISI5DPAlmcy", // Manager1234!
            RoleId = 3, EmployeeId = 2, IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new User
        {
            Id = 4, FullName = "Ayşe Kaya", Email = "employee@demo.com",
            PasswordHash = "$2a$11$uDROGAG.L2IJ/zDLlS/UjOXaiCpVo7/QCchgwykadmU3HLEhjR/A2", // Employee1234!
            RoleId = 4, EmployeeId = 1, IsActive = true, CreatedAt = DateTime.UtcNow
        },
    ];

    // ── Competencies ──────────────────────────────────────────────────────
    public static List<Competency> Competencies { get; } =
    [
        new Competency { Id = 1,  Code = "Core_Communication",  Name = "İletişim",             Category = "Core", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 2,  Code = "Core_Teamwork",       Name = "Takım Çalışması",       Category = "Core", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 3,  Code = "Core_ProblemSolving", Name = "Problem Çözme",         Category = "Core", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 4,  Code = "Core_Adaptability",   Name = "Uyum Yeteneği",         Category = "Core", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 5,  Code = "Core_Initiative",     Name = "İnisiyatif Alma",       Category = "Core", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 6,  Code = "Core_Accountability", Name = "Sorumluluk Bilinci",    Category = "Core", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 7,  Code = "Core_LearningAgility", Name = "Öğrenme Çevikliği",    Category = "Core", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 8,  Code = "Core_TimeManagement", Name = "Zaman Yönetimi",        Category = "Core", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 9,  Code = "Dept_Comp1",          Name = "Departman Yetkinliği 1",Category = "Department", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 10, Code = "Dept_Comp2",          Name = "Departman Yetkinliği 2",Category = "Department", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 11, Code = "Dept_Comp3",          Name = "Departman Yetkinliği 3",Category = "Department", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 12, Code = "Role_Comp1",          Name = "Rol Yetkinliği 1",      Category = "Role", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 13, Code = "Role_Comp2",          Name = "Rol Yetkinliği 2",      Category = "Role", IsActive = true, CreatedAt = DateTime.UtcNow },
    ];

    // ── Assessment Cycle ─────────────────────────────────────────────────
    public static List<AssessmentCycle> AssessmentCycles { get; } =
    [
        new AssessmentCycle
        {
            Id = 1, Name = "2024 Q4 Değerlendirmesi",
            StartDate = new DateTime(2024, 10, 1), EndDate = new DateTime(2024, 12, 31),
            Status = "Active", CreatedAt = DateTime.UtcNow
        }
    ];

    // ── Assessments ───────────────────────────────────────────────────────
    public static List<Assessment> Assessments { get; } =
    [
        new Assessment
        {
            Id = 1, EmployeeId = 1, CycleId = 1, OverallScore = 3.5,
            Status = Domain.Enums.AssessmentStatus.Completed, CreatedByUserId = 2,
            CreatedAt = DateTime.UtcNow
        }
    ];

    // ── Assessment Scores (360°: 4 evaluators × 13 competencies for Assessment 1) ──
    // Evaluators: Self=Emp1(Ayşe), Manager=Emp2(Mehmet), Peer=Emp3(Ali), Peer=Emp4(Zeynep)
    public static List<AssessmentScore> AssessmentScores { get; } =
    [
        // Self (EvaluatorEmployeeId = 1)
        new AssessmentScore { Id = 1,  AssessmentId = 1, CompetencyId = 1,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 2,  AssessmentId = 1, CompetencyId = 2,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.8, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 3,  AssessmentId = 1, CompetencyId = 3,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.6, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 4,  AssessmentId = 1, CompetencyId = 4,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.3, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 5,  AssessmentId = 1, CompetencyId = 5,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.2, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 6,  AssessmentId = 1, CompetencyId = 6,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.9, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 7,  AssessmentId = 1, CompetencyId = 7,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.6, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 8,  AssessmentId = 1, CompetencyId = 8,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.1, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 9,  AssessmentId = 1, CompetencyId = 9,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.7, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 10, AssessmentId = 1, CompetencyId = 10, EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.2, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 11, AssessmentId = 1, CompetencyId = 11, EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.8, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 12, AssessmentId = 1, CompetencyId = 12, EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.6, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 13, AssessmentId = 1, CompetencyId = 13, EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Self, Score = 3.3, CreatedAt = DateTime.UtcNow },
        // Manager (EvaluatorEmployeeId = 2)
        new AssessmentScore { Id = 14, AssessmentId = 1, CompetencyId = 1,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.2, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 15, AssessmentId = 1, CompetencyId = 2,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.7, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 16, AssessmentId = 1, CompetencyId = 3,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 17, AssessmentId = 1, CompetencyId = 4,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.1, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 18, AssessmentId = 1, CompetencyId = 5,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 19, AssessmentId = 1, CompetencyId = 6,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.8, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 20, AssessmentId = 1, CompetencyId = 7,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.4, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 21, AssessmentId = 1, CompetencyId = 8,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 2.9, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 22, AssessmentId = 1, CompetencyId = 9,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 23, AssessmentId = 1, CompetencyId = 10, EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 24, AssessmentId = 1, CompetencyId = 11, EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.6, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 25, AssessmentId = 1, CompetencyId = 12, EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.4, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 26, AssessmentId = 1, CompetencyId = 13, EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Manager, Score = 3.1, CreatedAt = DateTime.UtcNow },
        // Peer 1 — Ali Demir (EvaluatorEmployeeId = 3)
        new AssessmentScore { Id = 27, AssessmentId = 1, CompetencyId = 1,  EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 28, AssessmentId = 1, CompetencyId = 2,  EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 29, AssessmentId = 1, CompetencyId = 3,  EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.3, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 30, AssessmentId = 1, CompetencyId = 4,  EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 31, AssessmentId = 1, CompetencyId = 5,  EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 2.9, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 32, AssessmentId = 1, CompetencyId = 6,  EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.6, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 33, AssessmentId = 1, CompetencyId = 7,  EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.2, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 34, AssessmentId = 1, CompetencyId = 8,  EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 2.8, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 35, AssessmentId = 1, CompetencyId = 9,  EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.3, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 36, AssessmentId = 1, CompetencyId = 10, EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 2.9, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 37, AssessmentId = 1, CompetencyId = 11, EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.4, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 38, AssessmentId = 1, CompetencyId = 12, EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.2, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 39, AssessmentId = 1, CompetencyId = 13, EvaluatorEmployeeId = 3, EvaluatorType = EvaluatorType.Peer, Score = 3.0, CreatedAt = DateTime.UtcNow },
        // Peer 2 — Zeynep Arslan (EvaluatorEmployeeId = 4)
        new AssessmentScore { Id = 40, AssessmentId = 1, CompetencyId = 1,  EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.1, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 41, AssessmentId = 1, CompetencyId = 2,  EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.6, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 42, AssessmentId = 1, CompetencyId = 3,  EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.4, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 43, AssessmentId = 1, CompetencyId = 4,  EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.2, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 44, AssessmentId = 1, CompetencyId = 5,  EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 45, AssessmentId = 1, CompetencyId = 6,  EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.7, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 46, AssessmentId = 1, CompetencyId = 7,  EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.3, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 47, AssessmentId = 1, CompetencyId = 8,  EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 48, AssessmentId = 1, CompetencyId = 9,  EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.4, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 49, AssessmentId = 1, CompetencyId = 10, EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.1, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 50, AssessmentId = 1, CompetencyId = 11, EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 51, AssessmentId = 1, CompetencyId = 12, EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.3, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 52, AssessmentId = 1, CompetencyId = 13, EvaluatorEmployeeId = 4, EvaluatorType = EvaluatorType.Peer, Score = 3.1, CreatedAt = DateTime.UtcNow },
    ];

    // ── Assessment Assignments (360° evaluators for Assessment 1) ─────────────
    public static List<AssessmentAssignment> AssessmentAssignments { get; } =
    [
        new AssessmentAssignment { Id = 1, AssessmentId = 1, EvaluatorEmployeeId = 1, EvaluatorType = "Self",    IsCompleted = true,  CompletedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
        new AssessmentAssignment { Id = 2, AssessmentId = 1, EvaluatorEmployeeId = 2, EvaluatorType = "Manager", IsCompleted = true,  CompletedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
        new AssessmentAssignment { Id = 3, AssessmentId = 1, EvaluatorEmployeeId = 3, EvaluatorType = "Peer",    IsCompleted = true,  CompletedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
        new AssessmentAssignment { Id = 4, AssessmentId = 1, EvaluatorEmployeeId = 4, EvaluatorType = "Peer",    IsCompleted = false, CompletedAt = null,            CreatedAt = DateTime.UtcNow },
    ];

    // ── Action Catalog (all 47 ML action labels, shared with the EF Core seed) ─────
    public static List<ActionCatalog> ActionCatalog { get; } = [.. ActionCatalogSeed.All];

    // ── Model Version ─────────────────────────────────────────────────────
    public static List<ModelVersion> ModelVersions { get; } =
    [
        new ModelVersion
        {
            Id = 1, ModelName = "LightGBM", Version = "Final_LightGBM_v1",
            Description = "LightGBM multi-label classifier for action recommendation",
            MicroF1 = 0.82, MacroF1 = 0.78, RocAuc = 0.91, HammingLoss = 0.09,
            IsActive = true, CreatedAt = DateTime.UtcNow
        }
    ];

    // ── Refresh Tokens (starts empty, filled at runtime) ─────────────────
    public static List<RefreshToken> RefreshTokens { get; } = [];

    // ── Action Plans (starts empty) ───────────────────────────────────────
    public static List<ActionPlan> ActionPlans { get; } = [];
    public static List<ActionPlanItem> ActionPlanItems { get; } = [];

    // ── AI Prediction data (starts empty) ─────────────────────────────────
    public static List<AiPredictionRun> AiPredictionRuns { get; } = [];
    public static List<AiPredictedAction> AiPredictedActions { get; } = [];

    // ── Employee Tasks (starts empty) ─────────────────────────────────────
    public static List<EmployeeTask> EmployeeTasks { get; } = [];

    // ── ID counters ────────────────────────────────────────────────────────
    public static int NextUserId { get; set; } = 10;
    public static int NextEmployeeId { get; set; } = 10;
    public static int NextAssessmentId { get; set; } = 10;
    public static int NextAssessmentScoreId { get; set; } = 100;
    public static int NextAssessmentAssignmentId { get; set; } = 10;
    public static int NextRefreshTokenId { get; set; } = 1;
    public static int NextActionPlanId { get; set; } = 1;
    public static int NextActionPlanItemId { get; set; } = 1;
    public static int NextAiRunId { get; set; } = 1;
    public static int NextAiActionId { get; set; } = 1;
    public static int NextEmployeeTaskId { get; set; } = 1;
}
