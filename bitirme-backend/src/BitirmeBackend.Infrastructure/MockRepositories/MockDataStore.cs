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
        new Role { Id = 1, Name = "Admin",    Description = "Full system access",         CreatedAt = DateTime.UtcNow },
        new Role { Id = 2, Name = "HR",       Description = "Human resources management", CreatedAt = DateTime.UtcNow },
        new Role { Id = 3, Name = "Manager",  Description = "Team management",            CreatedAt = DateTime.UtcNow },
        new Role { Id = 4, Name = "Employee", Description = "Standard employee access",   CreatedAt = DateTime.UtcNow },
    ];

    // ── Departments ───────────────────────────────────────────────────────
    public static List<Department> Departments { get; } =
    [
        new Department { Id = 1, Name = "Human Resources",      Code = "HR",    CreatedAt = DateTime.UtcNow },
        new Department { Id = 2, Name = "Technology",           Code = "TECH",  CreatedAt = DateTime.UtcNow },
        new Department { Id = 3, Name = "Sales & Marketing",    Code = "SALES", CreatedAt = DateTime.UtcNow },
        new Department { Id = 4, Name = "Finance & Accounting", Code = "FIN",   CreatedAt = DateTime.UtcNow },
        new Department { Id = 5, Name = "Operations",           Code = "OPS",   CreatedAt = DateTime.UtcNow },
    ];

    // ── Job Roles ─────────────────────────────────────────────────────────
    public static List<JobRole> JobRoles { get; } =
    [
        new JobRole { Id = 1,  Name = "HR Specialist",           DepartmentId = 1, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 2,  Name = "Recruiter",               DepartmentId = 1, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 3,  Name = "HR Manager",              DepartmentId = 1, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 4,  Name = "Software Engineer",       DepartmentId = 2, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 5,  Name = "Senior Software Engineer", DepartmentId = 2, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 6,  Name = "Data Scientist",          DepartmentId = 2, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 11, Name = "Engineering Manager",     DepartmentId = 2, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 12, Name = "Sales Executive",         DepartmentId = 3, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 13, Name = "Sales Representative",    DepartmentId = 3, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 14, Name = "Account Manager",         DepartmentId = 3, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 15, Name = "Marketing Specialist",    DepartmentId = 3, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 16, Name = "Accountant",              DepartmentId = 4, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 17, Name = "Financial Analyst",       DepartmentId = 4, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 18, Name = "Payroll Specialist",      DepartmentId = 4, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 19, Name = "Finance Manager",         DepartmentId = 4, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 20, Name = "Operations Specialist",   DepartmentId = 5, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 21, Name = "Logistics Coordinator",   DepartmentId = 5, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 22, Name = "Production Engineer",     DepartmentId = 5, CreatedAt = DateTime.UtcNow },
        new JobRole { Id = 24, Name = "Operations Manager",      DepartmentId = 5, CreatedAt = DateTime.UtcNow },
    ];

    // ── Employees ─────────────────────────────────────────────────────────
    public static List<Employee> Employees { get; } =
    [
        new Employee
        {
            Id = 1, EmployeeCode = "EMP001", FullName = "Ahmet Yılmaz",
            Email = "ahmet.yilmaz@demo.com", Age = 42, Gender = "Male",
            DepartmentId = 1, JobRoleId = 3, ManagerId = null,
            Education = "4", EducationField = "Human Resources",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Married",
            DistanceFromHome = 8, EnvironmentSatisfaction = 4, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 18, YearsAtCompany = 8,
            YearsInCurrentRole = 4, YearsWithCurrManager = 0,
            PerformanceScore = 4.2, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 2, EmployeeCode = "EMP002", FullName = "Buse Demir",
            Email = "buse.demir@demo.com", Age = 27, Gender = "Female",
            DepartmentId = 1, JobRoleId = 1, ManagerId = 1,
            Education = "3", EducationField = "Psychology",
            BusinessTravel = "Non-Travel", MaritalStatus = "Single",
            DistanceFromHome = 3, EnvironmentSatisfaction = 3, JobSatisfaction = 4,
            WorkLifeBalance = 4, TotalWorkingYears = 5, YearsAtCompany = 3,
            YearsInCurrentRole = 2, YearsWithCurrManager = 2,
            PerformanceScore = 3.8, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 3, EmployeeCode = "EMP003", FullName = "Cem Aydın",
            Email = "cem.aydin@demo.com", Age = 31, Gender = "Male",
            DepartmentId = 1, JobRoleId = 2, ManagerId = 1,
            Education = "3", EducationField = "Business Administration",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Married",
            DistanceFromHome = 12, EnvironmentSatisfaction = 3, JobSatisfaction = 3,
            WorkLifeBalance = 3, TotalWorkingYears = 8, YearsAtCompany = 4,
            YearsInCurrentRole = 3, YearsWithCurrManager = 3,
            PerformanceScore = 3.9, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 4, EmployeeCode = "EMP004", FullName = "Deniz Yıldız",
            Email = "deniz.yildiz@demo.com", Age = 29, Gender = "Female",
            DepartmentId = 1, JobRoleId = 1, ManagerId = 1,
            Education = "3", EducationField = "Sociology",
            BusinessTravel = "Non-Travel", MaritalStatus = "Single",
            DistanceFromHome = 6, EnvironmentSatisfaction = 2, JobSatisfaction = 3,
            WorkLifeBalance = 3, TotalWorkingYears = 6, YearsAtCompany = 2,
            YearsInCurrentRole = 2, YearsWithCurrManager = 2,
            PerformanceScore = 3.5, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 5, EmployeeCode = "EMP005", FullName = "Zeynep Arslan",
            Email = "zeynep.arslan@demo.com", Age = 39, Gender = "Female",
            DepartmentId = 2, JobRoleId = 11, ManagerId = null,
            Education = "4", EducationField = "Computer Science",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Married",
            DistanceFromHome = 15, EnvironmentSatisfaction = 4, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 16, YearsAtCompany = 6,
            YearsInCurrentRole = 3, YearsWithCurrManager = 0,
            PerformanceScore = 4.5, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 6, EmployeeCode = "EMP006", FullName = "Emre Koç",
            Email = "emre.koc@demo.com", Age = 26, Gender = "Male",
            DepartmentId = 2, JobRoleId = 4, ManagerId = 5,
            Education = "3", EducationField = "Computer Science",
            BusinessTravel = "Non-Travel", MaritalStatus = "Single",
            DistanceFromHome = 4, EnvironmentSatisfaction = 3, JobSatisfaction = 3,
            WorkLifeBalance = 2, TotalWorkingYears = 4, YearsAtCompany = 2,
            YearsInCurrentRole = 2, YearsWithCurrManager = 2,
            PerformanceScore = 3.6, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 7, EmployeeCode = "EMP007", FullName = "Elif Şen",
            Email = "elif.sen@demo.com", Age = 34, Gender = "Female",
            DepartmentId = 2, JobRoleId = 5, ManagerId = 5,
            Education = "3", EducationField = "Information Technology",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Married",
            DistanceFromHome = 10, EnvironmentSatisfaction = 4, JobSatisfaction = 4,
            WorkLifeBalance = 4, TotalWorkingYears = 11, YearsAtCompany = 5,
            YearsInCurrentRole = 2, YearsWithCurrManager = 3,
            PerformanceScore = 4.1, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 8, EmployeeCode = "EMP008", FullName = "Fatih Kaya",
            Email = "fatih.kaya@demo.com", Age = 30, Gender = "Male",
            DepartmentId = 2, JobRoleId = 6, ManagerId = 5,
            Education = "4", EducationField = "Mathematics",
            BusinessTravel = "Non-Travel", MaritalStatus = "Single",
            DistanceFromHome = 5, EnvironmentSatisfaction = 3, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 7, YearsAtCompany = 3,
            YearsInCurrentRole = 2, YearsWithCurrManager = 2,
            PerformanceScore = 3.9, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 9, EmployeeCode = "EMP009", FullName = "Hakan Çelik",
            Email = "hakan.celik@demo.com", Age = 45, Gender = "Male",
            DepartmentId = 3, JobRoleId = 12, ManagerId = null,
            Education = "3", EducationField = "Marketing",
            BusinessTravel = "Travel_Frequently", MaritalStatus = "Married",
            DistanceFromHome = 20, EnvironmentSatisfaction = 3, JobSatisfaction = 3,
            WorkLifeBalance = 2, TotalWorkingYears = 22, YearsAtCompany = 12,
            YearsInCurrentRole = 6, YearsWithCurrManager = 0,
            PerformanceScore = 4.0, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 10, EmployeeCode = "EMP010", FullName = "Gizem Öztürk",
            Email = "gizem.ozturk@demo.com", Age = 28, Gender = "Female",
            DepartmentId = 3, JobRoleId = 13, ManagerId = 9,
            Education = "3", EducationField = "Communication Studies",
            BusinessTravel = "Travel_Frequently", MaritalStatus = "Single",
            DistanceFromHome = 7, EnvironmentSatisfaction = 4, JobSatisfaction = 3,
            WorkLifeBalance = 3, TotalWorkingYears = 6, YearsAtCompany = 3,
            YearsInCurrentRole = 3, YearsWithCurrManager = 3,
            PerformanceScore = 3.7, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 11, EmployeeCode = "EMP011", FullName = "Hakan Yılmaz",
            Email = "hakan.yilmaz@demo.com", Age = 35, Gender = "Male",
            DepartmentId = 3, JobRoleId = 14, ManagerId = 9,
            Education = "3", EducationField = "Business",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Married",
            DistanceFromHome = 11, EnvironmentSatisfaction = 3, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 12, YearsAtCompany = 5,
            YearsInCurrentRole = 3, YearsWithCurrManager = 3,
            PerformanceScore = 3.9, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 12, EmployeeCode = "EMP012", FullName = "İrem Aslan",
            Email = "irem.aslan@demo.com", Age = 32, Gender = "Female",
            DepartmentId = 3, JobRoleId = 15, ManagerId = 9,
            Education = "4", EducationField = "Marketing",
            BusinessTravel = "Non-Travel", MaritalStatus = "Single",
            DistanceFromHome = 4, EnvironmentSatisfaction = 3, JobSatisfaction = 4,
            WorkLifeBalance = 4, TotalWorkingYears = 9, YearsAtCompany = 4,
            YearsInCurrentRole = 2, YearsWithCurrManager = 2,
            PerformanceScore = 4.1, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 13, EmployeeCode = "EMP013", FullName = "Kemal Şahin",
            Email = "kemal.sahin@demo.com", Age = 48, Gender = "Male",
            DepartmentId = 4, JobRoleId = 19, ManagerId = null,
            Education = "4", EducationField = "Finance",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Married",
            DistanceFromHome = 9, EnvironmentSatisfaction = 4, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 24, YearsAtCompany = 10,
            YearsInCurrentRole = 5, YearsWithCurrManager = 0,
            PerformanceScore = 4.4, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 14, EmployeeCode = "EMP014", FullName = "Lale Bulut",
            Email = "lale.bulut@demo.com", Age = 33, Gender = "Female",
            DepartmentId = 4, JobRoleId = 16, ManagerId = 13,
            Education = "3", EducationField = "Accounting",
            BusinessTravel = "Non-Travel", MaritalStatus = "Married",
            DistanceFromHome = 5, EnvironmentSatisfaction = 3, JobSatisfaction = 3,
            WorkLifeBalance = 4, TotalWorkingYears = 10, YearsAtCompany = 6,
            YearsInCurrentRole = 4, YearsWithCurrManager = 4,
            PerformanceScore = 3.8, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 15, EmployeeCode = "EMP015", FullName = "Murat Güler",
            Email = "murat.guler@demo.com", Age = 29, Gender = "Male",
            DepartmentId = 4, JobRoleId = 17, ManagerId = 13,
            Education = "3", EducationField = "Economics",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Single",
            DistanceFromHome = 8, EnvironmentSatisfaction = 3, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 6, YearsAtCompany = 3,
            YearsInCurrentRole = 2, YearsWithCurrManager = 2,
            PerformanceScore = 4.0, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 16, EmployeeCode = "EMP016", FullName = "Nalan Demir",
            Email = "nalan.demir@demo.com", Age = 36, Gender = "Female",
            DepartmentId = 4, JobRoleId = 18, ManagerId = 13,
            Education = "3", EducationField = "Business Administration",
            BusinessTravel = "Non-Travel", MaritalStatus = "Married",
            DistanceFromHome = 13, EnvironmentSatisfaction = 4, JobSatisfaction = 3,
            WorkLifeBalance = 3, TotalWorkingYears = 13, YearsAtCompany = 7,
            YearsInCurrentRole = 5, YearsWithCurrManager = 5,
            PerformanceScore = 3.7, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 17, EmployeeCode = "EMP017", FullName = "Orhan Yalçın",
            Email = "orhan.yalcin@demo.com", Age = 44, Gender = "Male",
            DepartmentId = 5, JobRoleId = 24, ManagerId = null,
            Education = "4", EducationField = "Industrial Engineering",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Married",
            DistanceFromHome = 10, EnvironmentSatisfaction = 3, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 20, YearsAtCompany = 8,
            YearsInCurrentRole = 4, YearsWithCurrManager = 0,
            PerformanceScore = 4.3, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 18, EmployeeCode = "EMP018", FullName = "Pınar Tekin",
            Email = "pinar.tekin@demo.com", Age = 28, Gender = "Female",
            DepartmentId = 5, JobRoleId = 20, ManagerId = 17,
            Education = "3", EducationField = "Business",
            BusinessTravel = "Non-Travel", MaritalStatus = "Single",
            DistanceFromHome = 3, EnvironmentSatisfaction = 3, JobSatisfaction = 3,
            WorkLifeBalance = 4, TotalWorkingYears = 5, YearsAtCompany = 3,
            YearsInCurrentRole = 2, YearsWithCurrManager = 2,
            PerformanceScore = 3.6, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 19, EmployeeCode = "EMP019", FullName = "Rıza Mutlu",
            Email = "riza.mutlu@demo.com", Age = 37, Gender = "Male",
            DepartmentId = 5, JobRoleId = 21, ManagerId = 17,
            Education = "3", EducationField = "Logistics",
            BusinessTravel = "Travel_Rarely", MaritalStatus = "Married",
            DistanceFromHome = 18, EnvironmentSatisfaction = 3, JobSatisfaction = 3,
            WorkLifeBalance = 3, TotalWorkingYears = 14, YearsAtCompany = 5,
            YearsInCurrentRole = 3, YearsWithCurrManager = 3,
            PerformanceScore = 3.9, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
        },
        new Employee
        {
            Id = 20, EmployeeCode = "EMP020", FullName = "Selin Yılmaz",
            Email = "selin.yilmaz@demo.com", Age = 31, Gender = "Female",
            DepartmentId = 5, JobRoleId = 22, ManagerId = 17,
            Education = "3", EducationField = "Mechanical Engineering",
            BusinessTravel = "Non-Travel", MaritalStatus = "Married",
            DistanceFromHome = 5, EnvironmentSatisfaction = 4, JobSatisfaction = 4,
            WorkLifeBalance = 3, TotalWorkingYears = 8, YearsAtCompany = 4,
            YearsInCurrentRole = 3, YearsWithCurrManager = 3,
            PerformanceScore = 4.0, Attrition = "No", IsActive = true, CreatedAt = DateTime.UtcNow
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
        // 20 employee users — password: Demo1234!
        new User { Id = 3,  FullName = "Ahmet Yılmaz", Email = "ahmet.yilmaz@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 3, EmployeeId = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 4,  FullName = "Buse Demir", Email = "buse.demir@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 5,  FullName = "Cem Aydın", Email = "cem.aydin@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 6,  FullName = "Deniz Yıldız", Email = "deniz.yildiz@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 7,  FullName = "Zeynep Arslan", Email = "zeynep.arslan@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 3, EmployeeId = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 8,  FullName = "Emre Koç", Email = "emre.koc@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 6, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 9,  FullName = "Elif Şen", Email = "elif.sen@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 7, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 10, FullName = "Fatih Kaya", Email = "fatih.kaya@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 8, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 11, FullName = "Hakan Çelik", Email = "hakan.celik@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 3, EmployeeId = 9, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 12, FullName = "Gizem Öztürk", Email = "gizem.ozturk@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 10, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 13, FullName = "Hakan Yılmaz", Email = "hakan.yilmaz@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 11, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 14, FullName = "İrem Aslan", Email = "irem.aslan@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 12, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 15, FullName = "Kemal Şahin", Email = "kemal.sahin@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 3, EmployeeId = 13, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 16, FullName = "Lale Bulut", Email = "lale.bulut@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 14, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 17, FullName = "Murat Güler", Email = "murat.guler@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 15, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 18, FullName = "Nalan Demir", Email = "nalan.demir@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 16, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 19, FullName = "Orhan Yalçın", Email = "orhan.yalcin@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 3, EmployeeId = 17, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 20, FullName = "Pınar Tekin", Email = "pinar.tekin@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 18, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 21, FullName = "Rıza Mutlu", Email = "riza.mutlu@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 19, IsActive = true, CreatedAt = DateTime.UtcNow },
        new User { Id = 22, FullName = "Selin Yılmaz", Email = "selin.yilmaz@demo.com",
            PasswordHash = "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC",
            RoleId = 4, EmployeeId = 20, IsActive = true, CreatedAt = DateTime.UtcNow },
    ];

    // ── Competencies ──────────────────────────────────────────────────────
    public static List<Competency> Competencies { get; } =
    [
        new Competency { Id = 1,  Code = "Core_Communication",   Name = "İletişim",               Category = "Core",       IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 2,  Code = "Core_Teamwork",        Name = "Takım Çalışması",         Category = "Core",       IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 3,  Code = "Core_ProblemSolving",  Name = "Problem Çözme",           Category = "Core",       IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 4,  Code = "Core_Adaptability",    Name = "Uyum Yeteneği",           Category = "Core",       IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 5,  Code = "Core_Initiative",      Name = "İnisiyatif Alma",         Category = "Core",       IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 6,  Code = "Core_Accountability",  Name = "Sorumluluk Bilinci",      Category = "Core",       IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 7,  Code = "Core_LearningAgility", Name = "Öğrenme Çevikliği",       Category = "Core",       IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 8,  Code = "Core_TimeManagement",  Name = "Zaman Yönetimi",          Category = "Core",       IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 9,  Code = "Dept_Comp1",           Name = "Departman Yetkinliği 1",  Category = "Department", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 10, Code = "Dept_Comp2",           Name = "Departman Yetkinliği 2",  Category = "Department", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 11, Code = "Dept_Comp3",           Name = "Departman Yetkinliği 3",  Category = "Department", IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 12, Code = "Role_Comp1",           Name = "Rol Yetkinliği 1",        Category = "Role",       IsActive = true, CreatedAt = DateTime.UtcNow },
        new Competency { Id = 13, Code = "Role_Comp2",           Name = "Rol Yetkinliği 2",        Category = "Role",       IsActive = true, CreatedAt = DateTime.UtcNow },
    ];

    // ── Assessment Cycle ─────────────────────────────────────────────────
    public static List<AssessmentCycle> AssessmentCycles { get; } =
    [
        new AssessmentCycle
        {
            Id = 1, Name = "2025 Yılı Q4 Değerlendirmesi",
            StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2025, 12, 31),
            Status = "Completed", CreatedAt = DateTime.UtcNow
        }
    ];

    // ── Assessments ───────────────────────────────────────────────────────
    public static List<Assessment> Assessments { get; } =
    [
        new Assessment
        {
            Id = 1, EmployeeId = 2, CycleId = 1, OverallScore = 3.8,
            Status = AssessmentStatus.Completed, CreatedByUserId = 2,
            CreatedAt = DateTime.UtcNow
        }
    ];

    // ── Assessment Assignments ─────────────────────────────────────────────
    public static List<AssessmentAssignment> AssessmentAssignments { get; } =
    [
        new AssessmentAssignment { Id = 1, AssessmentId = 1, EvaluatorEmployeeId = 2, EvaluatorType = "Self",    IsCompleted = true, CompletedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
        new AssessmentAssignment { Id = 2, AssessmentId = 1, EvaluatorEmployeeId = 1, EvaluatorType = "Manager", IsCompleted = true, CompletedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
    ];

    // ── Assessment Scores ─────────────────────────────────────────────────
    public static List<AssessmentScore> AssessmentScores { get; } =
    [
        // Self — Buse Demir (EvaluatorEmployeeId = 2)
        new AssessmentScore { Id = 1,  AssessmentId = 1, CompetencyId = 1,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 2,  AssessmentId = 1, CompetencyId = 2,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 4.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 3,  AssessmentId = 1, CompetencyId = 3,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.8, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 4,  AssessmentId = 1, CompetencyId = 4,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.2, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 5,  AssessmentId = 1, CompetencyId = 5,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 6,  AssessmentId = 1, CompetencyId = 6,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.9, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 7,  AssessmentId = 1, CompetencyId = 7,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.6, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 8,  AssessmentId = 1, CompetencyId = 8,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.1, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 9,  AssessmentId = 1, CompetencyId = 9,  EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.7, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 10, AssessmentId = 1, CompetencyId = 10, EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.3, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 11, AssessmentId = 1, CompetencyId = 11, EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.8, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 12, AssessmentId = 1, CompetencyId = 12, EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 13, AssessmentId = 1, CompetencyId = 13, EvaluatorEmployeeId = 2, EvaluatorType = EvaluatorType.Self, Score = 3.4, CreatedAt = DateTime.UtcNow },
        // Manager — Ahmet Yılmaz (EvaluatorEmployeeId = 1)
        new AssessmentScore { Id = 14, AssessmentId = 1, CompetencyId = 1,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 3.8, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 15, AssessmentId = 1, CompetencyId = 2,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 4.2, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 16, AssessmentId = 1, CompetencyId = 3,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 4.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 17, AssessmentId = 1, CompetencyId = 4,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 3.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 18, AssessmentId = 1, CompetencyId = 5,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 3.6, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 19, AssessmentId = 1, CompetencyId = 6,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 4.1, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 20, AssessmentId = 1, CompetencyId = 7,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 3.9, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 21, AssessmentId = 1, CompetencyId = 8,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 3.7, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 22, AssessmentId = 1, CompetencyId = 9,  EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 4.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 23, AssessmentId = 1, CompetencyId = 10, EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 3.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 24, AssessmentId = 1, CompetencyId = 11, EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 4.0, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 25, AssessmentId = 1, CompetencyId = 12, EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 4.5, CreatedAt = DateTime.UtcNow },
        new AssessmentScore { Id = 26, AssessmentId = 1, CompetencyId = 13, EvaluatorEmployeeId = 1, EvaluatorType = EvaluatorType.Manager, Score = 3.8, CreatedAt = DateTime.UtcNow },
    ];

    // ── Action Catalog ────────────────────────────────────────────────────
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
    public static int NextUserId { get; set; } = 30;
    public static int NextEmployeeId { get; set; } = 30;
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
