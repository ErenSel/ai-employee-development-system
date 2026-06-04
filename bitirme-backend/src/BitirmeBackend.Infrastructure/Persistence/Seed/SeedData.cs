using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Persistence.Seed;

/// <summary>
/// Deterministic seed data applied via EF Core HasData (lands in the InitialCreate migration).
/// Demo credentials match the BCrypt hashes used by the original mock repositories:
///   admin@demo.com / Admin1234!   hr@demo.com / Hr1234!
///   manager@demo.com / Manager1234!   employee@demo.com / Employee1234!
/// Departments and job roles intentionally match the ContentData keys in db.sql so the
/// demo employee (Sales &amp; Marketing / Sales Executive) resolves nested catalog content.
/// </summary>
public static class SeedData
{
    private static readonly DateTime Now = SeedConstants.SeedDate;

    public static void Apply(ModelBuilder b)
    {
        b.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin",    Description = "Full system access",         CreatedAt = Now },
            new Role { Id = 2, Name = "HR",       Description = "Human resources management", CreatedAt = Now },
            new Role { Id = 3, Name = "Manager",  Description = "Team management",            CreatedAt = Now },
            new Role { Id = 4, Name = "Employee", Description = "Standard employee access",   CreatedAt = Now });

        b.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Technology",           Code = "TECH",  CreatedAt = Now },
            new Department { Id = 2, Name = "Human Resources",      Code = "HR",    CreatedAt = Now },
            new Department { Id = 3, Name = "Sales & Marketing",    Code = "SALES", CreatedAt = Now },
            new Department { Id = 4, Name = "Finance & Accounting", Code = "FIN",   CreatedAt = Now },
            new Department { Id = 5, Name = "Operations",           Code = "OPS",   CreatedAt = Now });

        b.Entity<JobRole>().HasData(
            new JobRole { Id = 1, Name = "Software Engineer",  DepartmentId = 1, CreatedAt = Now },
            new JobRole { Id = 2, Name = "Data Scientist",     DepartmentId = 1, CreatedAt = Now },
            new JobRole { Id = 3, Name = "HR Specialist",      DepartmentId = 2, CreatedAt = Now },
            new JobRole { Id = 4, Name = "Sales Executive",    DepartmentId = 3, CreatedAt = Now },
            new JobRole { Id = 5, Name = "Operations Manager", DepartmentId = 5, CreatedAt = Now },
            new JobRole { Id = 6, Name = "Accountant",         DepartmentId = 4, CreatedAt = Now });

        b.Entity<Competency>().HasData(
            new Competency { Id = 1,  Code = "Core_Communication",  Name = "İletişim",              Category = "Core",       IsActive = true, CreatedAt = Now },
            new Competency { Id = 2,  Code = "Core_Teamwork",       Name = "Takım Çalışması",       Category = "Core",       IsActive = true, CreatedAt = Now },
            new Competency { Id = 3,  Code = "Core_ProblemSolving", Name = "Problem Çözme",         Category = "Core",       IsActive = true, CreatedAt = Now },
            new Competency { Id = 4,  Code = "Core_Adaptability",   Name = "Uyum Yeteneği",         Category = "Core",       IsActive = true, CreatedAt = Now },
            new Competency { Id = 5,  Code = "Core_Initiative",     Name = "İnisiyatif Alma",       Category = "Core",       IsActive = true, CreatedAt = Now },
            new Competency { Id = 6,  Code = "Core_Accountability", Name = "Sorumluluk Bilinci",    Category = "Core",       IsActive = true, CreatedAt = Now },
            new Competency { Id = 7,  Code = "Core_LearningAgility", Name = "Öğrenme Çevikliği",    Category = "Core",       IsActive = true, CreatedAt = Now },
            new Competency { Id = 8,  Code = "Core_TimeManagement", Name = "Zaman Yönetimi",        Category = "Core",       IsActive = true, CreatedAt = Now },
            new Competency { Id = 9,  Code = "Dept_Comp1",          Name = "Departman Yetkinliği 1", Category = "Department", IsActive = true, CreatedAt = Now },
            new Competency { Id = 10, Code = "Dept_Comp2",          Name = "Departman Yetkinliği 2", Category = "Department", IsActive = true, CreatedAt = Now },
            new Competency { Id = 11, Code = "Dept_Comp3",          Name = "Departman Yetkinliği 3", Category = "Department", IsActive = true, CreatedAt = Now },
            new Competency { Id = 12, Code = "Role_Comp1",          Name = "Rol Yetkinliği 1",      Category = "Role",       IsActive = true, CreatedAt = Now },
            new Competency { Id = 13, Code = "Role_Comp2",          Name = "Rol Yetkinliği 2",      Category = "Role",       IsActive = true, CreatedAt = Now });

        b.Entity<ModelVersion>().HasData(
            new ModelVersion
            {
                Id = 1, ModelName = "LightGBM", Version = "Final_LightGBM_v1",
                Description = "LightGBM multi-label classifier for action recommendation",
                MicroF1 = 0.9004, RocAuc = 0.9580,
                IsActive = true, CreatedAt = Now
            });

        b.Entity<AssessmentCycle>().HasData(
            new AssessmentCycle
            {
                Id = 1, Name = "2025 Yılı Q4 Değerlendirmesi",
                StartDate = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                Status = "Completed", CreatedAt = Now
            });

        b.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1, EmployeeCode = "EMP001", FullName = "Ayşe Kaya",
                Email = "employee@demo.com", Age = 32, Gender = "Female",
                DepartmentId = 3, JobRoleId = 4, ManagerId = 2,
                Education = "3", EducationField = "Life Sciences",
                BusinessTravel = "Travel_Rarely", MaritalStatus = "Single",
                DistanceFromHome = 5, EnvironmentSatisfaction = 3, JobSatisfaction = 4,
                WorkLifeBalance = 3, TotalWorkingYears = 8, YearsAtCompany = 5,
                YearsInCurrentRole = 3, YearsWithCurrManager = 2,
                PerformanceScore = 3.5, Attrition = "No", IsActive = true, CreatedAt = Now
            },
            new Employee
            {
                Id = 2, EmployeeCode = "MGR001", FullName = "Mehmet Yılmaz",
                Email = "manager@demo.com", Age = 40, Gender = "Male",
                DepartmentId = 3, JobRoleId = 4, ManagerId = null,
                Education = "4", EducationField = "Business",
                BusinessTravel = "Travel_Frequently", MaritalStatus = "Married",
                DistanceFromHome = 10, EnvironmentSatisfaction = 4, JobSatisfaction = 4,
                WorkLifeBalance = 3, TotalWorkingYears = 15, YearsAtCompany = 8,
                YearsInCurrentRole = 5, YearsWithCurrManager = 3,
                PerformanceScore = 4.2, Attrition = "No", IsActive = true, CreatedAt = Now
            },
            new Employee
            {
                Id = 3, EmployeeCode = "EMP002", FullName = "Ali Demir",
                Email = "ali.demir@demo.com", Age = 28, Gender = "Male",
                DepartmentId = 1, JobRoleId = 1, ManagerId = null,
                Education = "4", EducationField = "Computer Science",
                BusinessTravel = "Non-Travel", MaritalStatus = "Single",
                DistanceFromHome = 3, EnvironmentSatisfaction = 4, JobSatisfaction = 3,
                WorkLifeBalance = 4, TotalWorkingYears = 5, YearsAtCompany = 3,
                YearsInCurrentRole = 2, YearsWithCurrManager = 2,
                PerformanceScore = 3.8, Attrition = "No", IsActive = true, CreatedAt = Now
            });

        b.Entity<User>().HasData(
            new User
            {
                Id = 1, FullName = "Admin User", Email = "admin@demo.com",
                PasswordHash = "$2a$11$zfRkFzsfjJ/dMsTdAhq.ie1gKjkUH6hRdzo.8yAZTQLLWgpwhQ4QC", // Admin1234!
                RoleId = 1, EmployeeId = null, IsActive = true, CreatedAt = Now
            },
            new User
            {
                Id = 2, FullName = "HR User", Email = "hr@demo.com",
                PasswordHash = "$2a$11$nCoV4NjJ9bj4oP5K3riM2e4gNOPO0Kk3rKmwWZI2ne6gXrnPM.Rym", // Hr1234!
                RoleId = 2, EmployeeId = null, IsActive = true, CreatedAt = Now
            },
            new User
            {
                Id = 3, FullName = "Mehmet Yılmaz", Email = "manager@demo.com",
                PasswordHash = "$2a$11$/EjKk6BgJtPAZjDW3H9zmeHaUMH.nbtF11lhEvhE7dISI5DPAlmcy", // Manager1234!
                RoleId = 3, EmployeeId = 2, IsActive = true, CreatedAt = Now
            },
            new User
            {
                Id = 4, FullName = "Ayşe Kaya", Email = "employee@demo.com",
                PasswordHash = "$2a$11$uDROGAG.L2IJ/zDLlS/UjOXaiCpVo7/QCchgwykadmU3HLEhjR/A2", // Employee1234!
                RoleId = 4, EmployeeId = 1, IsActive = true, CreatedAt = Now
            });

        b.Entity<Assessment>().HasData(
            new Assessment
            {
                Id = 1, EmployeeId = 1, CycleId = 1, OverallScore = 3.5,
                Status = AssessmentStatus.Completed, CreatedByUserId = 2, CreatedAt = Now
            });

        // 13 competency scores for the demo assessment (EvaluatorType = Manager).
        b.Entity<AssessmentScore>().HasData(
            new AssessmentScore { Id = 1,  AssessmentId = 1, CompetencyId = 1,  EvaluatorType = EvaluatorType.Manager, Score = 3.2, CreatedAt = Now },
            new AssessmentScore { Id = 2,  AssessmentId = 1, CompetencyId = 2,  EvaluatorType = EvaluatorType.Manager, Score = 3.7, CreatedAt = Now },
            new AssessmentScore { Id = 3,  AssessmentId = 1, CompetencyId = 3,  EvaluatorType = EvaluatorType.Manager, Score = 3.5, CreatedAt = Now },
            new AssessmentScore { Id = 4,  AssessmentId = 1, CompetencyId = 4,  EvaluatorType = EvaluatorType.Manager, Score = 3.1, CreatedAt = Now },
            new AssessmentScore { Id = 5,  AssessmentId = 1, CompetencyId = 5,  EvaluatorType = EvaluatorType.Manager, Score = 3.0, CreatedAt = Now },
            new AssessmentScore { Id = 6,  AssessmentId = 1, CompetencyId = 6,  EvaluatorType = EvaluatorType.Manager, Score = 3.8, CreatedAt = Now },
            new AssessmentScore { Id = 7,  AssessmentId = 1, CompetencyId = 7,  EvaluatorType = EvaluatorType.Manager, Score = 3.4, CreatedAt = Now },
            new AssessmentScore { Id = 8,  AssessmentId = 1, CompetencyId = 8,  EvaluatorType = EvaluatorType.Manager, Score = 2.9, CreatedAt = Now },
            new AssessmentScore { Id = 9,  AssessmentId = 1, CompetencyId = 9,  EvaluatorType = EvaluatorType.Manager, Score = 3.5, CreatedAt = Now },
            new AssessmentScore { Id = 10, AssessmentId = 1, CompetencyId = 10, EvaluatorType = EvaluatorType.Manager, Score = 3.0, CreatedAt = Now },
            new AssessmentScore { Id = 11, AssessmentId = 1, CompetencyId = 11, EvaluatorType = EvaluatorType.Manager, Score = 3.6, CreatedAt = Now },
            new AssessmentScore { Id = 12, AssessmentId = 1, CompetencyId = 12, EvaluatorType = EvaluatorType.Manager, Score = 3.4, CreatedAt = Now },
            new AssessmentScore { Id = 13, AssessmentId = 1, CompetencyId = 13, EvaluatorType = EvaluatorType.Manager, Score = 3.1, CreatedAt = Now });

        b.Entity<ActionCatalog>().HasData(ActionCatalogSeed.All);
    }
}
