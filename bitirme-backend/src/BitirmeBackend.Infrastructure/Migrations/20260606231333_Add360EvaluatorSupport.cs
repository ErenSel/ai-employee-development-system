using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BitirmeBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add360EvaluatorSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EvaluatorEmployeeId",
                table: "AssessmentScores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AssessmentAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    EvaluatorEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    EvaluatorType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentAssignments_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentAssignments_Employees_EvaluatorEmployeeId",
                        column: x => x.EvaluatorEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AssessmentAssignments",
                columns: new[] { "Id", "AssessmentId", "CompletedAt", "CreatedAt", "EvaluatorEmployeeId", "EvaluatorType", "IsCompleted", "IsDeleted", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Self", true, false, null },
                    { 2, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", true, false, null },
                    { 3, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", true, false, null }
                });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.5 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.7999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.6000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.2999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.2000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.8999999999999999 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.6000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.1000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.7000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.2000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.7999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.6000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "EvaluatorEmployeeId", "EvaluatorType", "Score" },
                values: new object[] { 1, "Self", 3.2999999999999998 });

            migrationBuilder.InsertData(
                table: "AssessmentScores",
                columns: new[] { "Id", "AssessmentId", "CompetencyId", "CreatedAt", "EvaluatorEmployeeId", "EvaluatorType", "IsDeleted", "Score", "UpdatedAt" },
                values: new object[,]
                {
                    { 14, 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.2000000000000002, null },
                    { 15, 1, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.7000000000000002, null },
                    { 16, 1, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.5, null },
                    { 17, 1, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.1000000000000001, null },
                    { 18, 1, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.0, null },
                    { 19, 1, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.7999999999999998, null },
                    { 20, 1, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.3999999999999999, null },
                    { 21, 1, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 2.8999999999999999, null },
                    { 22, 1, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.5, null },
                    { 23, 1, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.0, null },
                    { 24, 1, 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.6000000000000001, null },
                    { 25, 1, 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.3999999999999999, null },
                    { 26, 1, 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Manager", false, 3.1000000000000001, null },
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
                    { 39, 1, 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Peer", false, 3.0, null }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Age", "Attrition", "BusinessTravel", "CreatedAt", "DepartmentId", "DistanceFromHome", "Education", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "Gender", "IsActive", "IsDeleted", "JobRoleId", "JobSatisfaction", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "UpdatedAt", "WorkLifeBalance", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager" },
                values: new object[] { 4, 30, "No", "Travel_Rarely", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 7, "3", "Marketing", "zeynep.arslan@demo.com", "EMP003", 3, "Zeynep Arslan", "Female", true, false, 4, 4, 2, "Married", 3.6000000000000001, 7, null, 3, 4, 2, 2 });

            migrationBuilder.InsertData(
                table: "AssessmentAssignments",
                columns: new[] { "Id", "AssessmentId", "CompletedAt", "CreatedAt", "EvaluatorEmployeeId", "EvaluatorType", "IsCompleted", "IsDeleted", "UpdatedAt" },
                values: new object[] { 4, 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Peer", false, false, null });

            migrationBuilder.InsertData(
                table: "AssessmentScores",
                columns: new[] { "Id", "AssessmentId", "CompetencyId", "CreatedAt", "EvaluatorEmployeeId", "EvaluatorType", "IsDeleted", "Score", "UpdatedAt" },
                values: new object[,]
                {
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

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentScores_AssessmentId_CompetencyId_EvaluatorEmploye~",
                table: "AssessmentScores",
                columns: new[] { "AssessmentId", "CompetencyId", "EvaluatorEmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentScores_EvaluatorEmployeeId",
                table: "AssessmentScores",
                column: "EvaluatorEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAssignments_AssessmentId_EvaluatorEmployeeId",
                table: "AssessmentAssignments",
                columns: new[] { "AssessmentId", "EvaluatorEmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAssignments_EvaluatorEmployeeId",
                table: "AssessmentAssignments",
                column: "EvaluatorEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentScores_Employees_EvaluatorEmployeeId",
                table: "AssessmentScores",
                column: "EvaluatorEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentScores_Employees_EvaluatorEmployeeId",
                table: "AssessmentScores");

            migrationBuilder.DropTable(
                name: "AssessmentAssignments");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentScores_AssessmentId_CompetencyId_EvaluatorEmploye~",
                table: "AssessmentScores");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentScores_EvaluatorEmployeeId",
                table: "AssessmentScores");

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 26);

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

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "EvaluatorEmployeeId",
                table: "AssessmentScores");

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.2000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.7000000000000002 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.5 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.1000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.0 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.7999999999999998 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.3999999999999999 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 2.8999999999999999 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.5 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.0 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.6000000000000001 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.3999999999999999 });

            migrationBuilder.UpdateData(
                table: "AssessmentScores",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "EvaluatorType", "Score" },
                values: new object[] { "Manager", 3.1000000000000001 });
        }
    }
}
