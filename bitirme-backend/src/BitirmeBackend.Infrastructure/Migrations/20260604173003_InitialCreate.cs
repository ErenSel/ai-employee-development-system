using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BitirmeBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionCatalogs",
                columns: table => new
                {
                    ActionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TargetCompetency = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ActionCategory = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ActionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Difficulty = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EstimatedEffortHours = table.Column<int>(type: "integer", nullable: false),
                    MinScore = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false),
                    MaxScore = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false),
                    ContentData = table.Column<string>(type: "jsonb", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionCatalogs", x => x.ActionId);
                    table.CheckConstraint("CK_ActionCatalogs_EffortHours", "\"EstimatedEffortHours\" >= 0");
                    table.CheckConstraint("CK_ActionCatalogs_ScoreRange", "\"MinScore\" <= \"MaxScore\"");
                });

            migrationBuilder.CreateTable(
                name: "AssessmentCycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentCycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Version = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MicroF1 = table.Column<double>(type: "double precision", nullable: true),
                    MacroF1 = table.Column<double>(type: "double precision", nullable: true),
                    RocAuc = table.Column<double>(type: "double precision", nullable: true),
                    HammingLoss = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRoles_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    JobRoleId = table.Column<int>(type: "integer", nullable: false),
                    ManagerId = table.Column<int>(type: "integer", nullable: true),
                    Education = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EducationField = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BusinessTravel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MaritalStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DistanceFromHome = table.Column<int>(type: "integer", nullable: false),
                    EnvironmentSatisfaction = table.Column<int>(type: "integer", nullable: false),
                    JobSatisfaction = table.Column<int>(type: "integer", nullable: false),
                    WorkLifeBalance = table.Column<int>(type: "integer", nullable: false),
                    TotalWorkingYears = table.Column<int>(type: "integer", nullable: false),
                    YearsAtCompany = table.Column<int>(type: "integer", nullable: false),
                    YearsInCurrentRole = table.Column<int>(type: "integer", nullable: false),
                    YearsWithCurrManager = table.Column<int>(type: "integer", nullable: false),
                    PerformanceScore = table.Column<double>(type: "double precision", nullable: false),
                    Attrition = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.CheckConstraint("CK_Employees_Age", "\"Age\" > 0");
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_JobRoles_JobRoleId",
                        column: x => x.JobRoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    CycleId = table.Column<int>(type: "integer", nullable: false),
                    OverallScore = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_AssessmentCycles_CycleId",
                        column: x => x.CycleId,
                        principalTable: "AssessmentCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReplacedByTokenHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionPlans_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPlans_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPlans_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AiPredictionRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    ModelVersionId = table.Column<int>(type: "integer", nullable: false),
                    RequestedByUserId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiPredictionRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiPredictionRuns_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AiPredictionRuns_ModelVersions_ModelVersionId",
                        column: x => x.ModelVersionId,
                        principalTable: "ModelVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AiPredictionRuns_Users_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    CompetencyId = table.Column<int>(type: "integer", nullable: false),
                    EvaluatorType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentScores", x => x.Id);
                    table.CheckConstraint("CK_AssessmentScores_Score", "\"Score\" >= 0 AND \"Score\" <= 5");
                    table.ForeignKey(
                        name: "FK_AssessmentScores_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssessmentScores_Competencies_CompetencyId",
                        column: x => x.CompetencyId,
                        principalTable: "Competencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    EvaluatorType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CommentText = table.Column<string>(type: "text", nullable: false),
                    SentimentScore = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackComments_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AiPredictedActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PredictionRunId = table.Column<int>(type: "integer", nullable: false),
                    ActionCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Probability = table.Column<double>(type: "double precision", nullable: false),
                    RankOrder = table.Column<int>(type: "integer", nullable: false),
                    IsSelected = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiPredictedActions", x => x.Id);
                    table.CheckConstraint("CK_AiPredictedActions_Probability", "\"Probability\" >= 0 AND \"Probability\" <= 1");
                    table.ForeignKey(
                        name: "FK_AiPredictedActions_AiPredictionRuns_PredictionRunId",
                        column: x => x.PredictionRunId,
                        principalTable: "AiPredictionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionPlanItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActionPlanId = table.Column<int>(type: "integer", nullable: false),
                    ActionCatalogId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AiPredictedActionId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Resource = table.Column<string>(type: "text", nullable: true),
                    DeliveryType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Priority = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OrderNo = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionPlanItems_ActionCatalogs_ActionCatalogId",
                        column: x => x.ActionCatalogId,
                        principalTable: "ActionCatalogs",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPlanItems_ActionPlans_ActionPlanId",
                        column: x => x.ActionPlanId,
                        principalTable: "ActionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPlanItems_AiPredictedActions_AiPredictedActionId",
                        column: x => x.AiPredictedActionId,
                        principalTable: "AiPredictedActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActionPlanItemId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    AssignedByUserId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeTasks_ActionPlanItems_ActionPlanItemId",
                        column: x => x.ActionPlanItemId,
                        principalTable: "ActionPlanItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeTasks_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeTasks_Users_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CommentText = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskComments_EmployeeTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "EmployeeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ActionCatalogs",
                columns: new[] { "ActionId", "ActionCategory", "ActionType", "ContentData", "CreatedAt", "Difficulty", "EstimatedEffortHours", "IsActive", "IsDeleted", "MaxScore", "MinScore", "TargetCompetency", "UpdatedAt" },
                values: new object[,]
                {
                    { "CORE_ACCT_01", "Core", "Behavioral Practice", "{\"title\": \"Responsibility Ownership\", \"resource\": \"https://www.coursera.org/learn/accountability\", \"description\": \"İşlerin sahiplenilmesi ve çözüm üretme.\", \"delivery_type\": \"Guided Practice\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High", 24, true, false, 1.70m, 1.00m, "Core_Accountability", null },
                    { "CORE_ACCT_02", "Core", "Scenario Based Learning", "{\"title\": \"Handling Mistakes\", \"resource\": \"https://www.udemy.com/course/extreme-ownership/\", \"description\": \"Hata durumlarında sorumluluk alma vakaları.\", \"delivery_type\": \"Workshop\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium-High", 16, true, false, 2.50m, 1.80m, "Core_Accountability", null },
                    { "CORE_ACCT_03", "Core", "Micro Learning", "{\"title\": \"Commitment Tracking Habits\", \"resource\": \"https://www.youtube.com/watch?v=T_lCsqXhYhM\", \"description\": \"Verilen sözleri takip etme alışkanlıkları.\", \"delivery_type\": \"Digital Module\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 8, true, false, 3.20m, 2.60m, "Core_Accountability", null },
                    { "CORE_ACCT_04", "Core", "Reflection", "{\"title\": \"Accountability Self Review\", \"resource\": \"https://hbr.org/2020/11/how-to-hold-yourself-accountable\", \"description\": \"Sahiplenme düzeyinin analizi.\", \"delivery_type\": \"Self Reflection\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Low", 4, true, false, 4.00m, 3.30m, "Core_Accountability", null },
                    { "CORE_ADAPT_01", "Core", "Behavioral Practice", "{\"title\": \"Change Readiness Foundations\", \"resource\": \"https://www.coursera.org/learn/change-management\", \"description\": \"Yeni durumlara uyum sağlama egzersizleri.\", \"delivery_type\": \"Guided Practice\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High", 24, true, false, 1.70m, 1.00m, "Core_Adaptability", null },
                    { "CORE_ADAPT_02", "Core", "Scenario Based Learning", "{\"title\": \"Responding to Change Scenarios\", \"resource\": \"https://www.udemy.com/course/adaptability-in-the-workplace/\", \"description\": \"Beklenmeyen durumlarda hızlı uyum senaryoları.\", \"delivery_type\": \"Workshop\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium-High", 16, true, false, 2.50m, 1.80m, "Core_Adaptability", null },
                    { "CORE_ADAPT_03", "Core", "Micro Learning", "{\"title\": \"Flexibility & Adjustment Skills\", \"resource\": \"https://www.youtube.com/watch?v=xZ_vJzY2T0w\", \"description\": \"Yaklaşım değiştirme becerilerini destekleyen içerikler.\", \"delivery_type\": \"Digital Module\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 8, true, false, 3.20m, 2.60m, "Core_Adaptability", null },
                    { "CORE_ADAPT_04", "Core", "Reflection", "{\"title\": \"Adaptability Self Assessment\", \"resource\": \"https://medium.com/adaptability-self-assessment\", \"description\": \"Değişimlere verilen tepkilerin öz analizi.\", \"delivery_type\": \"Self Reflection\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Low", 4, true, false, 4.00m, 3.30m, "Core_Adaptability", null },
                    { "CORE_COMM_01", "Core", "Behavioral Practice", "{\"title\": \"Structured Communication Fundamentals\", \"resource\": \"https://www.coursera.org/learn/effective-communication-specialization\", \"description\": \"Mesaj netliği ve amaç belirleme üzerine haftalık pratikler.\", \"delivery_type\": \"Self-guided + Practice\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High", 24, true, false, 1.70m, 1.00m, "Core_Communication", null },
                    { "CORE_COMM_02", "Core", "Guided Exercise", "{\"title\": \"Effective Feedback Application\", \"resource\": \"https://www.udemy.com/course/giving-and-receiving-feedback-at-work/\", \"description\": \"Açık ifade ve duygu yönetimi üzerine egzersizler.\", \"delivery_type\": \"Workshop\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium-High", 16, true, false, 2.50m, 1.80m, "Core_Communication", null },
                    { "CORE_COMM_03", "Core", "Micro Learning", "{\"title\": \"Clarity & Conciseness Tune-Up\", \"resource\": \"https://www.youtube.com/watch?v=HAnw168huqA\", \"description\": \"Mesajları sadeleştirme ve ana mesajı öne çıkarma alıştırmaları.\", \"delivery_type\": \"Digital Module\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 8, true, false, 3.20m, 2.60m, "Core_Communication", null },
                    { "CORE_COMM_04", "Core", "Reflection", "{\"title\": \"Communication Self-Review\", \"resource\": \"https://hbr.org/2022/11/a-checklist-for-better-communication\", \"description\": \"İletişim örneklerini değerlendirme ve kısa analiz.\", \"delivery_type\": \"Self Reflection\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Low", 4, true, false, 4.00m, 3.30m, "Core_Communication", null },
                    { "CORE_INIT_01", "Core", "Behavioral Practice", "{\"title\": \"Proactive Mindset Activation\", \"resource\": \"https://www.udemy.com/course/taking-initiative-at-work/\", \"description\": \"Sorumluluk alma ve aksiyon başlatma egzersizleri.\", \"delivery_type\": \"Guided Practice\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High", 24, true, false, 1.70m, 1.00m, "Core_Initiative", null },
                    { "CORE_INIT_02", "Core", "Scenario Based Learning", "{\"title\": \"Taking Ownership\", \"resource\": \"https://www.linkedin.com/learning/developing-resourcefulness\", \"description\": \"Yönlendirme olmayan durumlarda sorumluluk senaryoları.\", \"delivery_type\": \"Workshop\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium-High", 16, true, false, 2.50m, 1.80m, "Core_Initiative", null },
                    { "CORE_INIT_03", "Core", "Micro Learning", "{\"title\": \"Opportunity Spotting Habits\", \"resource\": \"https://www.youtube.com/watch?v=3M_E-J_3j8s\", \"description\": \"Geliştirme fırsatlarını fark etme içerikleri.\", \"delivery_type\": \"Digital Module\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 8, true, false, 3.20m, 2.60m, "Core_Initiative", null },
                    { "CORE_INIT_04", "Core", "Reflection", "{\"title\": \"Initiative Self Check\", \"resource\": \"https://hbr.org/2021/08/when-to-take-initiative-at-work\", \"description\": \"Alınan inisiyatiflerin öz değerlendirmesi.\", \"delivery_type\": \"Self Reflection\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Low", 4, true, false, 4.00m, 3.30m, "Core_Initiative", null },
                    { "CORE_LEARN_01", "Core", "Behavioral Practice", "{\"title\": \"Learning Habit Foundations\", \"resource\": \"https://www.coursera.org/learn/learning-how-to-learn\", \"description\": \"Yeni bilgi edinme ve işe uyarlama görevleri.\", \"delivery_type\": \"Guided Practice\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High", 24, true, false, 1.70m, 1.00m, "Core_LearningAgility", null },
                    { "CORE_LEARN_02", "Core", "Scenario Based Learning", "{\"title\": \"Learning from New Situations\", \"resource\": \"https://www.udemy.com/course/growth-mindset/\", \"description\": \"Bilinmeyen konularda hızlı öğrenme senaryoları.\", \"delivery_type\": \"Workshop\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium-High", 16, true, false, 2.50m, 1.80m, "Core_LearningAgility", null },
                    { "CORE_LEARN_03", "Core", "Micro Learning", "{\"title\": \"Continuous Improvement\", \"resource\": \"https://www.youtube.com/watch?v=pN34FNbOKXc\", \"description\": \"Kısa sürede bilgi edinme içerikleri.\", \"delivery_type\": \"Digital Module\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 8, true, false, 3.20m, 2.60m, "Core_LearningAgility", null },
                    { "CORE_LEARN_04", "Core", "Reflection", "{\"title\": \"Learning Agility Self Check\", \"resource\": \"https://hbr.org/2015/09/improve-your-ability-to-learn\", \"description\": \"Yeni konuların işe yansıtılması analizi.\", \"delivery_type\": \"Self Reflection\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Low", 4, true, false, 4.00m, 3.30m, "Core_LearningAgility", null },
                    { "CORE_PROB_01", "Core", "Analytical Practice", "{\"title\": \"Problem Decomposition Fundamentals\", \"resource\": \"https://www.coursera.org/learn/problem-solving\", \"description\": \"Problemleri alt bileşenlerine ayırma çalışmaları.\", \"delivery_type\": \"Guided Practice\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High", 24, true, false, 1.70m, 1.00m, "Core_ProblemSolving", null },
                    { "CORE_PROB_02", "Core", "Scenario Based Learning", "{\"title\": \"Root Cause Analysis Practice\", \"resource\": \"https://www.udemy.com/course/root-cause-analysis/\", \"description\": \"Yüzeysel belirtiler yerine temel nedenleri belirleme vakaları.\", \"delivery_type\": \"Workshop\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium-High", 16, true, false, 2.50m, 1.80m, "Core_ProblemSolving", null },
                    { "CORE_PROB_03", "Core", "Micro Learning", "{\"title\": \"Solution Evaluation Techniques\", \"resource\": \"https://www.youtube.com/watch?v=JhhC_kP11mI\", \"description\": \"Çözüm alternatiflerini değerlendirme alıştırmaları.\", \"delivery_type\": \"Digital Module\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 8, true, false, 3.20m, 2.60m, "Core_ProblemSolving", null },
                    { "CORE_PROB_04", "Core", "Reflection", "{\"title\": \"Problem-Solving Self Review\", \"resource\": \"https://hbr.org/2018/03/how-to-solve-tough-problems\", \"description\": \"Çözülen problemlerin yaklaşımını gözden geçirme.\", \"delivery_type\": \"Self Reflection\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Low", 4, true, false, 4.00m, 3.30m, "Core_ProblemSolving", null },
                    { "CORE_TEAM_01", "Core", "Behavioral Practice", "{\"title\": \"Collaboration Basics Immersion\", \"resource\": \"https://www.coursera.org/learn/teamwork-skills-effective-communication\", \"description\": \"Takım içi rol paylaşımı ve ortak hedef takibi görevleri.\", \"delivery_type\": \"Guided Practice\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High", 24, true, false, 1.70m, 1.00m, "Core_Teamwork", null },
                    { "CORE_TEAM_02", "Core", "Scenario Based Learning", "{\"title\": \"Conflict Handling in Teams\", \"resource\": \"https://www.pluralsight.com/courses/conflict-resolution-workplace\", \"description\": \"Anlaşmazlıklarda iş birliğini koruma üzerine senaryolar.\", \"delivery_type\": \"Workshop\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium-High", 16, true, false, 2.50m, 1.80m, "Core_Teamwork", null },
                    { "CORE_TEAM_03", "Core", "Micro Learning", "{\"title\": \"Effective Collaboration Habits\", \"resource\": \"https://www.youtube.com/watch?v=1qE5L2z5T0M\", \"description\": \"Bilgi paylaşımı ve destek alışkanlıklarını güçlendirme.\", \"delivery_type\": \"Digital Module\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 8, true, false, 3.20m, 2.60m, "Core_Teamwork", null },
                    { "CORE_TEAM_04", "Core", "Reflection", "{\"title\": \"Team Contribution Review\", \"resource\": \"https://medium.com/management-matters/the-ultimate-teamwork-checklist\", \"description\": \"Takım içindeki katkının ve destek düzeyinin öz değerlendirmesi.\", \"delivery_type\": \"Self Reflection\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Low", 4, true, false, 4.00m, 3.30m, "Core_Teamwork", null },
                    { "CORE_TIME_01", "Core", "Behavioral Practice", "{\"title\": \"Time Awareness & Priority Reset\", \"resource\": \"https://www.coursera.org/learn/work-smarter-not-harder\", \"description\": \"Önceliklendirme ve planlama alışkanlığı kazandırma.\", \"delivery_type\": \"Guided Practice\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High", 24, true, false, 1.70m, 1.00m, "Core_TimeManagement", null },
                    { "CORE_TIME_02", "Core", "Scenario Based Learning", "{\"title\": \"Managing Competing Priorities\", \"resource\": \"https://www.udemy.com/course/time-management-mastery/\", \"description\": \"İşler arasında öncelik belirleme vakaları.\", \"delivery_type\": \"Workshop\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium-High", 16, true, false, 2.50m, 1.80m, "Core_TimeManagement", null },
                    { "CORE_TIME_03", "Core", "Micro Learning", "{\"title\": \"Efficiency & Focus Techniques\", \"resource\": \"https://www.youtube.com/watch?v=snAhsXyO3Co\", \"description\": \"Odaklanma ve işleri tamamlama uygulamaları.\", \"delivery_type\": \"Digital Module\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 8, true, false, 3.20m, 2.60m, "Core_TimeManagement", null },
                    { "CORE_TIME_04", "Core", "Reflection", "{\"title\": \"Personal Time Review\", \"resource\": \"https://hbr.org/2019/01/how-to-actually-save-time\", \"description\": \"Zaman kullanım alışkanlıklarının değerlendirilmesi.\", \"delivery_type\": \"Self Reflection\"}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Low", 4, true, false, 4.00m, 3.30m, "Core_TimeManagement", null },
                    { "DEPT_COMP1_01", "Department", "PracticalTask", "{\"Operations\": {\"title\": \"Operasyon Planı (Ops_OperationsPlanning)\", \"resource\": \"https://www.smartsheet.com/free-weekly-schedule-templates\", \"delivery_type\": \"Task\"}, \"Technology\": {\"title\": \"PR Refactor (Tech_CodingQuality)\", \"resource\": \"https://github.com/collections/clean-code\", \"delivery_type\": \"Task\"}, \"Human Resources\": {\"title\": \"İlan Metni (HR_Recruitment)\", \"resource\": \"https://www.shrm.org/resourcesandtools/tools-and-samples/hr-forms/pages/jobposting_template.aspx\", \"delivery_type\": \"Task\"}, \"Sales & Marketing\": {\"title\": \"Müşteri Pitch (Sales_SalesSkill)\", \"resource\": \"https://blog.hubspot.com/sales/sales-pitch-examples\", \"delivery_type\": \"Task\"}, \"Finance & Accounting\": {\"title\": \"KPI Analizi (Fin_FinancialAnalysis)\", \"resource\": \"https://corporatefinanceinstitute.com/resources/excel/kpi-dashboard-template/\", \"delivery_type\": \"Task\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 3, true, false, 1.90m, 1.00m, "Dept_Comp1", null },
                    { "DEPT_COMP1_02", "Department", "Coaching", "{\"Operations\": {\"title\": \"Planlama Checklist\", \"resource\": \"https://safetyculture.com/checklists/operations-management/\", \"delivery_type\": \"Checklist\"}, \"Technology\": {\"title\": \"Kod İnceleme Checklist\", \"resource\": \"https://github.com/integrations/feature/code-review\", \"delivery_type\": \"Checklist\"}, \"Human Resources\": {\"title\": \"İşe Alım Checklist\", \"resource\": \"https://resources.workable.com/tutorial/recruitment-process-checklist\", \"delivery_type\": \"Checklist\"}, \"Sales & Marketing\": {\"title\": \"Satış Görüşmesi Checklist\", \"resource\": \"https://www.salesforce.com/resources/articles/sales-call-planning-checklist/\", \"delivery_type\": \"Checklist\"}, \"Finance & Accounting\": {\"title\": \"Kontrol Checklist\", \"resource\": \"https://www.aicpa.org/resources/toolkit/financial-statement-checklist\", \"delivery_type\": \"Checklist\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 2, true, false, 2.90m, 2.00m, "Dept_Comp1", null },
                    { "DEPT_COMP1_03", "Department", "MicroLearning", "{\"Operations\": {\"title\": \"Planlama 101\", \"resource\": \"https://www.coursera.org/learn/operations-management\", \"delivery_type\": \"Course\"}, \"Technology\": {\"title\": \"Kod Kalitesi\", \"resource\": \"https://www.pluralsight.com/courses/clean-code\", \"delivery_type\": \"Course\"}, \"Human Resources\": {\"title\": \"Mülakat Akışı\", \"resource\": \"https://www.coursera.org/learn/recruiting-hiring-onboarding-employees\", \"delivery_type\": \"Course\"}, \"Sales & Marketing\": {\"title\": \"Satış Temelleri\", \"resource\": \"https://www.udemy.com/course/sales-training-practical-sales-techniques/\", \"delivery_type\": \"Course\"}, \"Finance & Accounting\": {\"title\": \"Veri Okuma\", \"resource\": \"https://www.coursera.org/learn/financial-accounting-basics\", \"delivery_type\": \"Course\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Easy", 2, true, false, 4.00m, 3.00m, "Dept_Comp1", null },
                    { "DEPT_COMP2_01", "Department", "Simulation", "{\"Operations\": {\"title\": \"Teslimat Senaryosu\", \"resource\": \"https://www.edx.org/course/supply-chain-logistics\", \"delivery_type\": \"Exercise\"}, \"Technology\": {\"title\": \"Ölçeklenebilir Sistem Senaryosu\", \"resource\": \"https://github.com/donnemartin/system-design-primer\", \"delivery_type\": \"Exercise\"}, \"Human Resources\": {\"title\": \"Disiplin Süreci Senaryosu\", \"resource\": \"https://www.shrm.org/resourcesandtools/tools-and-samples/hr-forms/pages/disciplinary-action-form.aspx\", \"delivery_type\": \"Exercise\"}, \"Sales & Marketing\": {\"title\": \"Pazarlık Senaryosu\", \"resource\": \"https://www.udemy.com/course/negotiation-secrets-for-master-negotiators/\", \"delivery_type\": \"Exercise\"}, \"Finance & Accounting\": {\"title\": \"Hata Bulma Senaryosu\", \"resource\": \"https://corporatefinanceinstitute.com/course/accounting-fundamentals/\", \"delivery_type\": \"Exercise\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hard", 5, true, false, 1.90m, 1.00m, "Dept_Comp2", null },
                    { "DEPT_COMP2_02", "Department", "MicroLearning", "{\"Operations\": {\"title\": \"Lojistik Süreçleri\", \"resource\": \"https://www.coursera.org/learn/supply-chain-management\", \"delivery_type\": \"Course\"}, \"Technology\": {\"title\": \"Sistem Tasarımı\", \"resource\": \"https://www.udacity.com/course/software-architecture-design--ud821\", \"delivery_type\": \"Course\"}, \"Human Resources\": {\"title\": \"İş Hukuku\", \"resource\": \"https://www.coursera.org/learn/human-resources-management\", \"delivery_type\": \"Course\"}, \"Sales & Marketing\": {\"title\": \"Müzakere Stratejileri\", \"resource\": \"https://www.coursera.org/learn/negotiation-skills\", \"delivery_type\": \"Course\"}, \"Finance & Accounting\": {\"title\": \"Muhasebe İlkeleri\", \"resource\": \"https://www.coursera.org/learn/financial-reporting\", \"delivery_type\": \"Course\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 4, true, false, 2.90m, 2.00m, "Dept_Comp2", null },
                    { "DEPT_COMP2_03", "Department", "Reflection", "{\"Operations\": {\"title\": \"Lojistik Değerlendirme\", \"resource\": \"https://miro.com/templates/logistics-planning/\", \"delivery_type\": \"Template\"}, \"Technology\": {\"title\": \"Tasarım Değerlendirme\", \"resource\": \"https://miro.com/templates/architecture-diagram/\", \"delivery_type\": \"Template\"}, \"Human Resources\": {\"title\": \"Politika Değerlendirme\", \"resource\": \"https://miro.com/templates/hr-strategy/\", \"delivery_type\": \"Template\"}, \"Sales & Marketing\": {\"title\": \"Müzakere Değerlendirme\", \"resource\": \"https://miro.com/templates/sales-pipeline/\", \"delivery_type\": \"Template\"}, \"Finance & Accounting\": {\"title\": \"Muhasebe Değerlendirme\", \"resource\": \"https://miro.com/templates/financial-analysis/\", \"delivery_type\": \"Template\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Easy", 2, true, false, 4.00m, 3.00m, "Dept_Comp2", null },
                    { "DEPT_COMP3_01", "Department", "Coaching", "{\"Operations\": {\"title\": \"Süreç Koçluğu\", \"resource\": \"https://kanbanize.com/lean-management/improvement/kaizen\", \"delivery_type\": \"Checklist\"}, \"Technology\": {\"title\": \"Test Planı Koçluğu\", \"resource\": \"https://www.guru99.com/software-testing-checklist.html\", \"delivery_type\": \"Checklist\"}, \"Human Resources\": {\"title\": \"Zor Çalışan Koçluğu\", \"resource\": \"https://hbr.org/2015/02/how-to-coach-a-difficult-employee\", \"delivery_type\": \"Checklist\"}, \"Sales & Marketing\": {\"title\": \"CRM Koçluğu\", \"resource\": \"https://www.hubspot.com/resources/crm-implementation-checklist\", \"delivery_type\": \"Checklist\"}, \"Finance & Accounting\": {\"title\": \"Bordro İnceleme\", \"resource\": \"https://www.adp.com/resources/articles-and-insights/articles/p/payroll-checklist.aspx\", \"delivery_type\": \"Checklist\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hard", 4, true, false, 1.90m, 1.00m, "Dept_Comp3", null },
                    { "DEPT_COMP3_02", "Department", "MicroLearning", "{\"Operations\": {\"title\": \"Optimizasyon\", \"resource\": \"https://www.coursera.org/learn/process-improvement\", \"delivery_type\": \"Course\"}, \"Technology\": {\"title\": \"QA Süreçleri\", \"resource\": \"https://www.udacity.com/course/software-testing--cs258\", \"delivery_type\": \"Course\"}, \"Human Resources\": {\"title\": \"Çalışan İlişkileri\", \"resource\": \"https://www.coursera.org/learn/employee-relations\", \"delivery_type\": \"Course\"}, \"Sales & Marketing\": {\"title\": \"CRM Temelleri\", \"resource\": \"https://www.coursera.org/learn/crm\", \"delivery_type\": \"Course\"}, \"Finance & Accounting\": {\"title\": \"Bordro Süreci\", \"resource\": \"https://www.udemy.com/course/payroll-management/\", \"delivery_type\": \"Course\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 3, true, false, 2.90m, 2.00m, "Dept_Comp3", null },
                    { "DEPT_COMP3_03", "Department", "MicroLearning", "{\"Operations\": {\"title\": \"Süreç Quiz\", \"resource\": \"https://sixsigmastudyguide.com/lean-six-sigma-quiz/\", \"delivery_type\": \"Course\"}, \"Technology\": {\"title\": \"QA Quiz\", \"resource\": \"https://www.istqb.org/certifications/agile-tester\", \"delivery_type\": \"Course\"}, \"Human Resources\": {\"title\": \"Employee Relations Quiz\", \"resource\": \"https://www.shrm.org/certification/quiz\", \"delivery_type\": \"Course\"}, \"Sales & Marketing\": {\"title\": \"CRM Quiz\", \"resource\": \"https://academy.hubspot.com/courses/inbound-sales\", \"delivery_type\": \"Course\"}, \"Finance & Accounting\": {\"title\": \"Bordro Quiz\", \"resource\": \"https://www.aicpa.org/resources/quiz\", \"delivery_type\": \"Course\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 3, true, false, 4.00m, 3.00m, "Dept_Comp3", null },
                    { "ROLE_COMP1_01", "Role", "Simulation", "{\"Accountant\": {\"title\": \"Finansal Model Simülasyonu\", \"resource\": \"https://corporatefinanceinstitute.com/resources/excel/financial-modeling-simulations/\", \"delivery_type\": \"Exercise\"}, \"HR Specialist\": {\"title\": \"Dokümantasyon Simülasyonu\", \"resource\": \"https://www.shrm.org/resourcesandtools/tools-and-samples/hr-forms/pages/hr-simulations.aspx\", \"delivery_type\": \"Exercise\"}, \"Data Scientist\": {\"title\": \"Model Simülasyonu\", \"resource\": \"https://www.kaggle.com/competitions\", \"delivery_type\": \"Exercise\"}, \"Sales Executive\": {\"title\": \"Kapanış Simülasyonu\", \"resource\": \"https://trailhead.salesforce.com/en/content/learn/trails/sales-cloud-basics\", \"delivery_type\": \"Exercise\"}, \"Software Engineer\": {\"title\": \"Hata Ayıklama Simülasyonu\", \"resource\": \"https://github.com/topics/coding-challenges\", \"delivery_type\": \"Exercise\"}, \"Operations Manager\": {\"title\": \"Planlama Simülasyonu\", \"resource\": \"https://www.edx.org/learn/operations-management\", \"delivery_type\": \"Exercise\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hard", 6, true, false, 1.90m, 1.00m, "Role_Comp1", null },
                    { "ROLE_COMP1_02", "Role", "PracticalTask", "{\"Accountant\": {\"title\": \"Defter Tutma Görevi\", \"resource\": \"https://www.journalofaccountancy.com/issues.html\", \"delivery_type\": \"Task\"}, \"HR Specialist\": {\"title\": \"Dokümantasyon Görevi\", \"resource\": \"https://www.shrm.org/resourcesandtools/tools-and-samples/policies/pages/default.aspx\", \"delivery_type\": \"Task\"}, \"Data Scientist\": {\"title\": \"Veri Model Görevi\", \"resource\": \"https://paperswithcode.com/sota\", \"delivery_type\": \"Task\"}, \"Sales Executive\": {\"title\": \"Pipeline Yönetimi\", \"resource\": \"https://blog.hubspot.com/sales/sales-strategy-template\", \"delivery_type\": \"Task\"}, \"Software Engineer\": {\"title\": \"Problem Parçalama\", \"resource\": \"https://github.com/search?q=architecture+katas\", \"delivery_type\": \"Task\"}, \"Operations Manager\": {\"title\": \"Operasyon Görevi\", \"resource\": \"https://www.smartsheet.com/operations-management-templates\", \"delivery_type\": \"Task\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 4, true, false, 2.90m, 2.00m, "Role_Comp1", null },
                    { "ROLE_COMP1_03", "Role", "MicroLearning", "{\"Accountant\": {\"title\": \"Muhasebe Eğitimi\", \"resource\": \"https://www.coursera.org/learn/financial-accounting-basics\", \"delivery_type\": \"Course\"}, \"HR Specialist\": {\"title\": \"Dokümantasyon Eğitimi\", \"resource\": \"https://www.coursera.org/learn/managing-human-resources\", \"delivery_type\": \"Course\"}, \"Data Scientist\": {\"title\": \"Veri Bilimi Eğitim\", \"resource\": \"https://www.coursera.org/specializations/machine-learning-introduction\", \"delivery_type\": \"Course\"}, \"Sales Executive\": {\"title\": \"Satış Eğitimi\", \"resource\": \"https://academy.hubspot.com/courses/inbound-sales\", \"delivery_type\": \"Course\"}, \"Software Engineer\": {\"title\": \"Mimari Eğitim\", \"resource\": \"https://www.pluralsight.com/courses/software-architecture-fundamentals\", \"delivery_type\": \"Course\"}, \"Operations Manager\": {\"title\": \"Yönetim Eğitimi\", \"resource\": \"https://www.coursera.org/learn/operations-management\", \"delivery_type\": \"Course\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Easy", 2, true, false, 4.00m, 3.00m, "Role_Comp1", null },
                    { "ROLE_COMP2_01", "Role", "PracticalTask", "{\"Accountant\": {\"title\": \"Mevzuat Görevi\", \"resource\": \"https://corporatefinanceinstitute.com/resources/templates/\", \"delivery_type\": \"Task\"}, \"HR Specialist\": {\"title\": \"İç Destek Görevi\", \"resource\": \"https://hbr.org/2020/01/hr-strategy-task-guide\", \"delivery_type\": \"Task\"}, \"Data Scientist\": {\"title\": \"Model Yorumlama\", \"resource\": \"https://www.kaggle.com/datasets\", \"delivery_type\": \"Task\"}, \"Sales Executive\": {\"title\": \"CRM Görevi\", \"resource\": \"https://www.salesforce.com/resources/articles/pipeline-management/\", \"delivery_type\": \"Task\"}, \"Software Engineer\": {\"title\": \"Mimari Görev\", \"resource\": \"https://github.com/collections/choosing-projects\", \"delivery_type\": \"Task\"}, \"Operations Manager\": {\"title\": \"İzleme Görevi\", \"resource\": \"https://safetyculture.com/templates/operations/\", \"delivery_type\": \"Task\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 3, true, false, 1.90m, 1.00m, "Role_Comp2", null },
                    { "ROLE_COMP2_02", "Role", "Coaching", "{\"Accountant\": {\"title\": \"Mevzuat Koçluk\", \"resource\": \"https://www.aicpa.org/resources/toolkit/month-end-close-checklist\", \"delivery_type\": \"Checklist\"}, \"HR Specialist\": {\"title\": \"İç Destek Koçluk\", \"resource\": \"https://resources.workable.com/hr-checklists\", \"delivery_type\": \"Checklist\"}, \"Data Scientist\": {\"title\": \"Model Koçluk\", \"resource\": \"https://developers.google.com/machine-learning/crash-course/production-ml-systems\", \"delivery_type\": \"Checklist\"}, \"Sales Executive\": {\"title\": \"CRM Koçluk\", \"resource\": \"https://blog.hubspot.com/sales/sales-coaching-checklist\", \"delivery_type\": \"Checklist\"}, \"Software Engineer\": {\"title\": \"Mimari Koçluk\", \"resource\": \"https://github.com/integrations/feature/code-review\", \"delivery_type\": \"Checklist\"}, \"Operations Manager\": {\"title\": \"İzleme Koçluk\", \"resource\": \"https://www.proces.st/operations-management-checklist/\", \"delivery_type\": \"Checklist\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medium", 2, true, false, 2.90m, 2.00m, "Role_Comp2", null },
                    { "ROLE_COMP2_03", "Role", "Reflection", "{\"Accountant\": {\"title\": \"Mevzuat Şablon\", \"resource\": \"https://miro.com/templates/budget-planning/\", \"delivery_type\": \"Template\"}, \"HR Specialist\": {\"title\": \"İç Destek Şablonu\", \"resource\": \"https://miro.com/templates/hr-retro/\", \"delivery_type\": \"Template\"}, \"Data Scientist\": {\"title\": \"Model Şablon\", \"resource\": \"https://miro.com/templates/data-model/\", \"delivery_type\": \"Template\"}, \"Sales Executive\": {\"title\": \"CRM Şablon\", \"resource\": \"https://miro.com/templates/sales-funnel/\", \"delivery_type\": \"Template\"}, \"Software Engineer\": {\"title\": \"Mimari Şablon\", \"resource\": \"https://miro.com/templates/sprint-retrospective/\", \"delivery_type\": \"Template\"}, \"Operations Manager\": {\"title\": \"İzleme Şablon\", \"resource\": \"https://miro.com/templates/lean-canvas/\", \"delivery_type\": \"Template\"}}", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Easy", 1, true, false, 4.00m, 3.00m, "Role_Comp2", null }
                });

            migrationBuilder.InsertData(
                table: "AssessmentCycles",
                columns: new[] { "Id", "CreatedAt", "EndDate", "IsDeleted", "Name", "StartDate", "Status", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), false, "2025 Yılı Q4 Değerlendirmesi", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Completed", null });

            migrationBuilder.InsertData(
                table: "Competencies",
                columns: new[] { "Id", "Category", "Code", "CreatedAt", "Description", "IsActive", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Core", "Core_Communication", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "İletişim", null },
                    { 2, "Core", "Core_Teamwork", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Takım Çalışması", null },
                    { 3, "Core", "Core_ProblemSolving", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Problem Çözme", null },
                    { 4, "Core", "Core_Adaptability", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Uyum Yeteneği", null },
                    { 5, "Core", "Core_Initiative", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "İnisiyatif Alma", null },
                    { 6, "Core", "Core_Accountability", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Sorumluluk Bilinci", null },
                    { 7, "Core", "Core_LearningAgility", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Öğrenme Çevikliği", null },
                    { 8, "Core", "Core_TimeManagement", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Zaman Yönetimi", null },
                    { 9, "Department", "Dept_Comp1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Departman Yetkinliği 1", null },
                    { 10, "Department", "Dept_Comp2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Departman Yetkinliği 2", null },
                    { 11, "Department", "Dept_Comp3", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Departman Yetkinliği 3", null },
                    { 12, "Role", "Role_Comp1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Rol Yetkinliği 1", null },
                    { 13, "Role", "Role_Comp2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Rol Yetkinliği 2", null }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "TECH", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Technology", null },
                    { 2, "HR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Human Resources", null },
                    { 3, "SALES", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Sales & Marketing", null },
                    { 4, "FIN", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Finance & Accounting", null },
                    { 5, "OPS", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Operations", null }
                });

            migrationBuilder.InsertData(
                table: "ModelVersions",
                columns: new[] { "Id", "CreatedAt", "Description", "HammingLoss", "IsActive", "IsDeleted", "MacroF1", "MicroF1", "ModelName", "RocAuc", "UpdatedAt", "Version" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LightGBM multi-label classifier for action recommendation", null, true, false, null, 0.90039999999999998, "LightGBM", 0.95799999999999996, null, "Final_LightGBM_v1" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full system access", false, "Admin", null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Human resources management", false, "HR", null },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Team management", false, "Manager", null },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Standard employee access", false, "Employee", null }
                });

            migrationBuilder.InsertData(
                table: "JobRoles",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, false, "Software Engineer", null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, false, "Data Scientist", null },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, false, "HR Specialist", null },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, false, "Sales Executive", null },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, false, "Operations Manager", null },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, false, "Accountant", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmployeeId", "FullName", "IsActive", "IsDeleted", "PasswordHash", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@demo.com", null, "Admin User", true, false, "$2a$11$zfRkFzsfjJ/dMsTdAhq.ie1gKjkUH6hRdzo.8yAZTQLLWgpwhQ4QC", 1, null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hr@demo.com", null, "HR User", true, false, "$2a$11$nCoV4NjJ9bj4oP5K3riM2e4gNOPO0Kk3rKmwWZI2ne6gXrnPM.Rym", 2, null }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Age", "Attrition", "BusinessTravel", "CreatedAt", "DepartmentId", "DistanceFromHome", "Education", "EducationField", "Email", "EmployeeCode", "EnvironmentSatisfaction", "FullName", "Gender", "IsActive", "IsDeleted", "JobRoleId", "JobSatisfaction", "ManagerId", "MaritalStatus", "PerformanceScore", "TotalWorkingYears", "UpdatedAt", "WorkLifeBalance", "YearsAtCompany", "YearsInCurrentRole", "YearsWithCurrManager" },
                values: new object[,]
                {
                    { 2, 40, "No", "Travel_Frequently", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 10, "4", "Business", "manager@demo.com", "MGR001", 4, "Mehmet Yılmaz", "Male", true, false, 4, 4, null, "Married", 4.2000000000000002, 15, null, 3, 8, 5, 3 },
                    { 3, 28, "No", "Non-Travel", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3, "4", "Computer Science", "ali.demir@demo.com", "EMP002", 4, "Ali Demir", "Male", true, false, 1, 3, null, "Single", 3.7999999999999998, 5, null, 4, 3, 2, 2 },
                    { 1, 32, "No", "Travel_Rarely", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 5, "3", "Life Sciences", "employee@demo.com", "EMP001", 3, "Ayşe Kaya", "Female", true, false, 4, 4, 2, "Single", 3.5, 8, null, 3, 5, 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmployeeId", "FullName", "IsActive", "IsDeleted", "PasswordHash", "RoleId", "UpdatedAt" },
                values: new object[] { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "manager@demo.com", 2, "Mehmet Yılmaz", true, false, "$2a$11$/EjKk6BgJtPAZjDW3H9zmeHaUMH.nbtF11lhEvhE7dISI5DPAlmcy", 3, null });

            migrationBuilder.InsertData(
                table: "Assessments",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "CycleId", "EmployeeId", "IsDeleted", "OverallScore", "Status", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1, 1, false, 3.5, "Completed", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmployeeId", "FullName", "IsActive", "IsDeleted", "PasswordHash", "RoleId", "UpdatedAt" },
                values: new object[] { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "employee@demo.com", 1, "Ayşe Kaya", true, false, "$2a$11$uDROGAG.L2IJ/zDLlS/UjOXaiCpVo7/QCchgwykadmU3HLEhjR/A2", 4, null });

            migrationBuilder.InsertData(
                table: "AssessmentScores",
                columns: new[] { "Id", "AssessmentId", "CompetencyId", "CreatedAt", "EvaluatorType", "IsDeleted", "Score", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.2000000000000002, null },
                    { 2, 1, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.7000000000000002, null },
                    { 3, 1, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.5, null },
                    { 4, 1, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.1000000000000001, null },
                    { 5, 1, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.0, null },
                    { 6, 1, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.7999999999999998, null },
                    { 7, 1, 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.3999999999999999, null },
                    { 8, 1, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 2.8999999999999999, null },
                    { 9, 1, 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.5, null },
                    { 10, 1, 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.0, null },
                    { 11, 1, 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.6000000000000001, null },
                    { 12, 1, 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.3999999999999999, null },
                    { 13, 1, 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", false, 3.1000000000000001, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPlanItems_ActionCatalogId",
                table: "ActionPlanItems",
                column: "ActionCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPlanItems_ActionPlanId",
                table: "ActionPlanItems",
                column: "ActionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPlanItems_AiPredictedActionId",
                table: "ActionPlanItems",
                column: "AiPredictedActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPlans_CreatedByUserId",
                table: "ActionPlans",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPlans_EmployeeId",
                table: "ActionPlans",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "UX_ActionPlans_AssessmentId_ActivePlan",
                table: "ActionPlans",
                column: "AssessmentId",
                unique: true,
                filter: "\"IsDeleted\" = false AND \"Status\" IN ('Draft', 'Edited', 'Approved', 'Sent')");

            migrationBuilder.CreateIndex(
                name: "IX_AiPredictedActions_PredictionRunId",
                table: "AiPredictedActions",
                column: "PredictionRunId");

            migrationBuilder.CreateIndex(
                name: "IX_AiPredictionRuns_AssessmentId",
                table: "AiPredictionRuns",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AiPredictionRuns_ModelVersionId",
                table: "AiPredictionRuns",
                column: "ModelVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AiPredictionRuns_RequestedByUserId",
                table: "AiPredictionRuns",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_CreatedByUserId",
                table: "Assessments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_CycleId",
                table: "Assessments",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_EmployeeId",
                table: "Assessments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentScores_AssessmentId",
                table: "AssessmentScores",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentScores_CompetencyId",
                table: "AssessmentScores",
                column: "CompetencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Competencies_Code",
                table: "Competencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobRoleId",
                table: "Employees",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerId",
                table: "Employees",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTasks_AssignedByUserId",
                table: "EmployeeTasks",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTasks_EmployeeId",
                table: "EmployeeTasks",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "UX_EmployeeTasks_ActionPlanItemId_Active",
                table: "EmployeeTasks",
                column: "ActionPlanItemId",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackComments_AssessmentId",
                table: "FeedbackComments",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRoles_DepartmentId",
                table: "JobRoles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_UserId",
                table: "TaskComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentScores");

            migrationBuilder.DropTable(
                name: "FeedbackComments");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "Competencies");

            migrationBuilder.DropTable(
                name: "EmployeeTasks");

            migrationBuilder.DropTable(
                name: "ActionPlanItems");

            migrationBuilder.DropTable(
                name: "ActionCatalogs");

            migrationBuilder.DropTable(
                name: "ActionPlans");

            migrationBuilder.DropTable(
                name: "AiPredictedActions");

            migrationBuilder.DropTable(
                name: "AiPredictionRuns");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "ModelVersions");

            migrationBuilder.DropTable(
                name: "AssessmentCycles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "JobRoles");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
