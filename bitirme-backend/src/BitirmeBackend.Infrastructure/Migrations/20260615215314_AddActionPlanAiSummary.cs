using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitirmeBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActionPlanAiSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AiSummary",
                table: "ActionPlans",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiSummary",
                table: "ActionPlans");
        }
    }
}
