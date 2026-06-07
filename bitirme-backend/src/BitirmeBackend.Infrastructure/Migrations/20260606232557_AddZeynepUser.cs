using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitirmeBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddZeynepUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmployeeId", "FullName", "IsActive", "IsDeleted", "PasswordHash", "RoleId", "UpdatedAt" },
                values: new object[] { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "zeynep.arslan@demo.com", 4, "Zeynep Arslan", true, false, "$2a$11$uDROGAG.L2IJ/zDLlS/UjOXaiCpVo7/QCchgwykadmU3HLEhjR/A2", 4, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
