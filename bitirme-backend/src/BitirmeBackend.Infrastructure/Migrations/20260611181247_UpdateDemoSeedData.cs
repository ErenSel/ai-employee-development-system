using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BitirmeBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDemoSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssessmentAssignments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AssessmentAssignments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.UpdateData(
                table: "AssessmentAssignments",
                keyColumn: "Id",
                keyValue: 1,
                column: "EvaluatorEmployeeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AssessmentAssignments",
                keyColumn: "Id",
                keyValue: 2,
                column: "EvaluatorEmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 1,
                column: "EvaluatorEmployeeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 4.0 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.7999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.2000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.0 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 6,
                column: "EvaluatorEmployeeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 7,
                column: "EvaluatorEmployeeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 8,
                column: "EvaluatorEmployeeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 9,
                column: "EvaluatorEmployeeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.2999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 11,
                column: "EvaluatorEmployeeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.5 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.3999999999999999 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.7999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 4.2000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 4.0 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.5 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.6000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 4.0999999999999996 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.8999999999999999 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.7000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 4.0 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.5 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 4.0 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 4.5 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.7999999999999998 });

            migrationBuilder.UpdateData(
                table: "Assessments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmployeeId", "OverallScore" },
                values: new object[] { 2, 3.7999999999999998 });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "Name" },
                values: new object[] { "HR", "Human Resources" });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Code", "Name" },
                values: new object[] { "TECH", "Technology" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Age", "DepartmentId", "DistanceFromHome", "Education", "EducationField", "Email", "EnvironmentSatisfaction", "FullName", "Gender", "JobRoleId", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager" },
                values: new object[] { 42, 1, 8, "4", "Human Resources", "ahmet.yilmaz@demo.com", 4, "Ahmet Yılmaz", "Male", 3, null, "Married", 4.2000000000000002, 18, 8, 4, 0 });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Age", "BusinessTravel", "DepartmentId", "DistanceFromHome", "Education", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "Gender", "JobRoleId", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "WorkLifeBalance", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager" },
                values: new object[] { 27, "Non-Travel", 1, 3, "3", "Psychology", "buse.demir@demo.com", "EMP002", 3, "Buse Demir", "Female", 1, 1, "Single", 3.7999999999999998, 5, 4, 3, 2, 2 });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Age", "BusinessTravel", "DistanceFromHome", "Education", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "JobRoleId", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "WorkLifeBalance", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager" },
                values: new object[] { 31, "Travel_Rarely", 12, "3", "Business Administration", "cem.aydin@demo.com", "EMP003", 3, "Cem Aydın", 2, 1, "Married", 3.8999999999999999, 8, 3, 4, 3, 3 });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Age", "BusinessTravel", "DepartmentId", "DistanceFromHome", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "JobRoleId", "JobSatisfaction", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "YearsAtCompany" },
                values: new object[] { 29, "Non-Travel", 1, 6, "Sociology", "deniz.yildiz@demo.com", "EMP004", 2, "Deniz Yıldız", 1, 3, 1, "Single", 3.5, 6, 2 });

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "HR Specialist");

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Recruiter");

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 1, "HR Manager" });

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 2, "Software Engineer" });

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 2, "Senior Software Engineer" });

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 2, "Data Scientist" });

            migrationBuilder.InsertData(
                table: "JobRoles",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, "Engineering Manager", null },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, false, "Sales Executive", null },
                    { 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, false, "Sales Representative", null },
                    { 14, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, false, "Account Manager", null },
                    { 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, false, "Marketing Specialist", null },
                    { 16, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, false, "Accountant", null },
                    { 17, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, false, "Financial Analyst", null },
                    { 18, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, false, "Payroll Specialist", null },
                    { 19, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, false, "Finance Manager", null },
                    { 20, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, false, "Operations Specialist", null },
                    { 21, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, false, "Logistics Coordinator", null },
                    { 22, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, false, "Production Engineer", null },
                    { 24, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, false, "Operations Manager", null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "EmployeeId", "FullName", "PasswordHash" },
                values: new object[] { "ahmet.yilmaz@demo.com", 1, "Ahmet Yılmaz", "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "EmployeeId", "FullName", "PasswordHash" },
                values: new object[] { "buse.demir@demo.com", 2, "Buse Demir", "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "EmployeeId", "FullName", "PasswordHash" },
                values: new object[] { "cem.aydin@demo.com", 3, "Cem Aydın", "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmployeeId", "FullName", "IsActive", "IsDeleted", "PasswordHash", "RoleId", "UpdatedAt" },
                values: new object[] { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "deniz.yildiz@demo.com", 4, "Deniz Yıldız", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Age", "Attrition", "BusinessTravel", "CreatedAt", "DepartmentId", "DistanceFromHome", "Education", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "Gender", "IsActive", "IsDeleted", "JobRoleId", "JobSatisfaction", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "UpdatedAt", "WorkLifeBalance", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager" },
                values: new object[,]
                {
                    { 5, 39, "No", "Travel_Rarely", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 15, "4", "Computer Science", "zeynep.arslan@demo.com", "EMP005", 4, "Zeynep Arslan", "Female", true, false, 11, 4, null, "Married", 4.5, 16, null, 3, 6, 3, 0 },
                    { 9, 45, "No", "Travel_Frequently", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 20, "3", "Marketing", "hakan.celik@demo.com", "EMP009", 3, "Hakan Çelik", "Male", true, false, 12, 3, null, "Married", 4.0, 22, null, 2, 12, 6, 0 },
                    { 13, 48, "No", "Travel_Rarely", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 9, "4", "Finance", "kemal.sahin@demo.com", "EMP013", 4, "Kemal Şahin", "Male", true, false, 19, 4, null, "Married", 4.4000000000000004, 24, null, 3, 10, 5, 0 },
                    { 17, 44, "No", "Travel_Rarely", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 10, "4", "Industrial Engineering", "orhan.yalcin@demo.com", "EMP017", 3, "Orhan Yalçın", "Male", true, false, 24, 4, null, "Married", 4.2999999999999998, 20, null, 3, 8, 4, 0 },
                    { 6, 26, "No", "Non-Travel", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 4, "3", "Computer Science", "emre.koc@demo.com", "EMP006", 3, "Emre Koç", "Male", true, false, 4, 3, 5, "Single", 3.6000000000000001, 4, null, 2, 2, 2, 2 },
                    { 7, 34, "No", "Travel_Rarely", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 10, "3", "Information Technology", "elif.sen@demo.com", "EMP007", 4, "Elif Şen", "Female", true, false, 5, 4, 5, "Married", 4.0999999999999996, 11, null, 4, 5, 2, 3 },
                    { 8, 30, "No", "Non-Travel", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 5, "4", "Mathematics", "fatih.kaya@demo.com", "EMP008", 3, "Fatih Kaya", "Male", true, false, 6, 4, 5, "Single", 3.8999999999999999, 7, null, 3, 3, 2, 2 },
                    { 10, 28, "No", "Travel_Frequently", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 7, "3", "Communication Studies", "gizem.ozturk@demo.com", "EMP010", 4, "Gizem Öztürk", "Female", true, false, 13, 3, 9, "Single", 3.7000000000000002, 6, null, 3, 3, 3, 3 },
                    { 11, 35, "No", "Travel_Rarely", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 11, "3", "Business", "hakan.yilmaz@demo.com", "EMP011", 3, "Hakan Yılmaz", "Male", true, false, 14, 4, 9, "Married", 3.8999999999999999, 12, null, 3, 5, 3, 3 },
                    { 12, 32, "No", "Non-Travel", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 4, "4", "Marketing", "irem.aslan@demo.com", "EMP012", 3, "İrem Aslan", "Female", true, false, 15, 4, 9, "Single", 4.0999999999999996, 9, null, 4, 4, 2, 2 },
                    { 14, 33, "No", "Non-Travel", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 5, "3", "Accounting", "lale.bulut@demo.com", "EMP014", 3, "Lale Bulut", "Female", true, false, 16, 3, 13, "Married", 3.7999999999999998, 10, null, 4, 6, 4, 4 },
                    { 15, 29, "No", "Travel_Rarely", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 8, "3", "Economics", "murat.guler@demo.com", "EMP015", 3, "Murat Güler", "Male", true, false, 17, 4, 13, "Single", 4.0, 6, null, 3, 3, 2, 2 },
                    { 16, 36, "No", "Non-Travel", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 13, "3", "Business Administration", "nalan.demir@demo.com", "EMP016", 4, "Nalan Demir", "Female", true, false, 18, 3, 13, "Married", 3.7000000000000002, 13, null, 3, 7, 5, 5 },
                    { 18, 28, "No", "Non-Travel", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 3, "3", "Business", "pinar.tekin@demo.com", "EMP018", 3, "Pınar Tekin", "Female", true, false, 20, 3, 17, "Single", 3.6000000000000001, 5, null, 4, 3, 2, 2 },
                    { 19, 37, "No", "Travel_Rarely", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 18, "3", "Logistics", "riza.mutlu@demo.com", "EMP019", 3, "Rıza Mutlu", "Male", true, false, 21, 3, 17, "Married", 3.8999999999999999, 14, null, 3, 5, 3, 3 },
                    { 20, 31, "No", "Non-Travel", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 5, "3", "Mechanical Engineering", "selin.yilmaz@demo.com", "EMP020", 4, "Selin Yılmaz", "Female", true, false, 22, 4, 17, "Married", 4.0, 8, null, 3, 4, 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmployeeId", "FullName", "IsActive", "IsDeleted", "PasswordHash", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "zeynep.arslan@demo.com", 5, "Zeynep Arslan", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 3, null },
                    { 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hakan.celik@demo.com", 9, "Hakan Çelik", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 3, null },
                    { 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "kemal.sahin@demo.com", 13, "Kemal Şahin", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 3, null },
                    { 19, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "orhan.yalcin@demo.com", 17, "Orhan Yalçın", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 3, null },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "emre.koc@demo.com", 6, "Emre Koç", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "elif.sen@demo.com", 7, "Elif Şen", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fatih.kaya@demo.com", 8, "Fatih Kaya", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "gizem.ozturk@demo.com", 10, "Gizem Öztürk", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hakan.yilmaz@demo.com", 11, "Hakan Yılmaz", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 14, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "irem.aslan@demo.com", 12, "İrem Aslan", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 16, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "lale.bulut@demo.com", 14, "Lale Bulut", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 17, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "murat.guler@demo.com", 15, "Murat Güler", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 18, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nalan.demir@demo.com", 16, "Nalan Demir", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 20, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pinar.tekin@demo.com", 18, "Pınar Tekin", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 21, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "riza.mutlu@demo.com", 19, "Rıza Mutlu", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null },
                    { 22, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "selin.yilmaz@demo.com", 20, "Selin Yılmaz", true, false, "$2a$11$rwGBgkTsyrgRtFIBIb7AdeLz5n6ISGcK2Fd1CXBydDLDlJZUElnuC", 4, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.UpdateData(
                table: "AssessmentAssignments",
                keyColumn: "Id",
                keyValue: 1,
                column: "EvaluatorEmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AssessmentAssignments",
                keyColumn: "Id",
                keyValue: 2,
                column: "EvaluatorEmployeeId",
                value: 2);

            migrationBuilder.InsertData(
                table: "AssessmentAssignments",
                columns: new[] { "Id", "AssessmentId", "CompletedAt", "CreatedAt", "EvaluatorEmployeeId", "EvaluatorType", "IsCompleted", "IsDeleted", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", true, false, null },
                    { 4, 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, false, null }
                });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 1,
                column: "EvaluatorEmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.7999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.6000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.2999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.2000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 6,
                column: "EvaluatorEmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 7,
                column: "EvaluatorEmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 8,
                column: "EvaluatorEmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 9,
                column: "EvaluatorEmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.2000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 11,
                column: "EvaluatorEmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.6000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 1, 3.2999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.2000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.7000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.5 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.1000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.0 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.7999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.3999999999999999 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 2.8999999999999999 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.5 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.0 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.6000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.3999999999999999 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "EvaluatorEmployeeId", "Score" },
                values: new object[] { 2, 3.1000000000000001 });

            migrationBuilder.InsertData(
                table: "AssessmentScores",
                columns: new[] { "Id", "AssessmentId", "CompetencyId", "CreatedAt", "EvaluatorEmployeeId", "EvaluatorType", "IsDeleted", "Score", "UpdatedAt" },
                values: new object[,]
                {
                    { 27, 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.0, null },
                    { 28, 1, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.5, null },
                    { 29, 1, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.2999999999999998, null },
                    { 30, 1, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.0, null },
                    { 31, 1, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 2.8999999999999999, null },
                    { 32, 1, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.6000000000000001, null },
                    { 33, 1, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.2000000000000002, null },
                    { 34, 1, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 2.7999999999999998, null },
                    { 35, 1, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.2999999999999998, null },
                    { 36, 1, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 2.8999999999999999, null },
                    { 37, 1, 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.3999999999999999, null },
                    { 38, 1, 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.2000000000000002, null },
                    { 39, 1, 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.0, null },
                    { 40, 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.1000000000000001, null },
                    { 41, 1, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.6000000000000001, null },
                    { 42, 1, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.3999999999999999, null },
                    { 43, 1, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.2000000000000002, null },
                    { 44, 1, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.0, null },
                    { 45, 1, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.7000000000000002, null },
                    { 46, 1, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.2999999999999998, null },
                    { 47, 1, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.0, null },
                    { 48, 1, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.3999999999999999, null },
                    { 49, 1, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.1000000000000001, null },
                    { 50, 1, 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.5, null },
                    { 51, 1, 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.2999999999999998, null },
                    { 52, 1, 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, 3.1000000000000001, null }
                });

            migrationBuilder.UpdateData(
                table: "Assessments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmployeeId", "OverallScore" },
                values: new object[] { 1, 3.5 });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "Name" },
                values: new object[] { "TECH", "Technology" });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Code", "Name" },
                values: new object[] { "HR", "Human Resources" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Age", "DepartmentId", "DistanceFromHome", "Education", "EducationField", "Email", "EnvironmentSatisfaction", "FullName", "Gender", "JobRoleId", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager" },
                values: new object[] { 32, 3, 5, "3", "Life Sciences", "employee@demo.com", 3, "Ayşe Kaya", "Female", 4, 2, "Single", 3.5, 8, 5, 3, 2 });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Age", "BusinessTravel", "DepartmentId", "DistanceFromHome", "Education", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "Gender", "JobRoleId", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "WorkLifeBalance", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager" },
                values: new object[] { 40, "Travel_Frequently", 3, 10, "4", "Business", "manager@demo.com", "MGR001", 4, "Mehmet Yılmaz", "Male", 4, null, "Married", 4.2000000000000002, 15, 3, 8, 5, 3 });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Age", "BusinessTravel", "DistanceFromHome", "Education", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "JobRoleId", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "WorkLifeBalance", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager" },
                values: new object[] { 28, "Non-Travel", 3, "4", "Computer Science", "ali.demir@demo.com", "EMP002", 4, "Ali Demir", 1, null, "Single", 3.7999999999999998, 5, 4, 3, 2, 2 });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Age", "BusinessTravel", "DepartmentId", "DistanceFromHome", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "JobRoleId", "JobSatisfaction", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "YearsAtCompany" },
                values: new object[] { 30, "Travel_Rarely", 3, 7, "Marketing", "zeynep.arslan@demo.com", "EMP003", 3, "Zeynep Arslan", 4, 4, 2, "Married", 3.6000000000000001, 7, 4 });

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Software Engineer");

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Data Scientist");

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 2, "HR Specialist" });

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 3, "Sales Executive" });

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 5, "Operations Manager" });

            migrationBuilder.UpdateData(
                table: "JobRoles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 4, "Accountant" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "EmployeeId", "FullName", "PasswordHash" },
                values: new object[] { "manager@demo.com", 2, "Mehmet Yılmaz", "$2a$11$/EjKk6BgJtPAZjDW3H9zmeHaUMH.nbtF11lhEvhE7dISI5DPAlmcy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "EmployeeId", "FullName", "PasswordHash" },
                values: new object[] { "employee@demo.com", 1, "Ayşe Kaya", "$2a$11$uDROGAG.L2IJ/zDLlS/UjOXaiCpVo7/QCchgwykadmU3HLEhjR/A2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "EmployeeId", "FullName", "PasswordHash" },
                values: new object[] { "zeynep.arslan@demo.com", 4, "Zeynep Arslan", "$2a$11$uDROGAG.L2IJ/zDLlS/UjOXaiCpVo7/QCchgwykadmU3HLEhjR/A2" });
        }
    }
}
