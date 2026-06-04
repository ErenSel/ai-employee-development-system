using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Persistence;

/// <summary>
/// EF Core database context for the PostgreSQL backend.
/// Entity schema lives in Persistence/Configurations (applied via assembly scan);
/// seed data lives in Persistence/Seed.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<JobRole> JobRoles => Set<JobRole>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<AssessmentCycle> AssessmentCycles => Set<AssessmentCycle>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<AssessmentScore> AssessmentScores => Set<AssessmentScore>();
    public DbSet<Competency> Competencies => Set<Competency>();
    public DbSet<FeedbackComment> FeedbackComments => Set<FeedbackComment>();
    public DbSet<ModelVersion> ModelVersions => Set<ModelVersion>();
    public DbSet<AiPredictionRun> AiPredictionRuns => Set<AiPredictionRun>();
    public DbSet<AiPredictedAction> AiPredictedActions => Set<AiPredictedAction>();
    public DbSet<ActionCatalog> ActionCatalogs => Set<ActionCatalog>();
    public DbSet<ActionPlan> ActionPlans => Set<ActionPlan>();
    public DbSet<ActionPlanItem> ActionPlanItems => Set<ActionPlanItem>();
    public DbSet<EmployeeTask> EmployeeTasks => Set<EmployeeTask>();
    public DbSet<TaskComment> TaskComments => Set<TaskComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        SeedData.Apply(modelBuilder);
    }
}
