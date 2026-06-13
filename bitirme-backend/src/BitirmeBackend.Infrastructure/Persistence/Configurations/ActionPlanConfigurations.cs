using BitirmeBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeBackend.Infrastructure.Persistence.Configurations;

public class ActionPlanConfiguration : IEntityTypeConfiguration<ActionPlan>
{
    public void Configure(EntityTypeBuilder<ActionPlan> b)
    {
        b.ToTable("ActionPlans");
        b.HasKey(x => x.Id);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.EmployeeId);

        // Only one active plan per assessment (Draft/Edited/Approved/Sent, not soft-deleted).
        b.HasIndex(x => x.AssessmentId)
            .IsUnique()
            .HasDatabaseName("UX_ActionPlans_AssessmentId_ActivePlan")
            .HasFilter("\"IsDeleted\" = false AND \"Status\" IN ('Draft', 'Edited', 'Approved', 'Sent')");

        b.HasOne(x => x.Assessment)
            .WithMany(a => a.ActionPlans)
            .HasForeignKey(x => x.AssessmentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Employee)
            .WithMany(e => e.ActionPlans)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ActionPlanItemConfiguration : IEntityTypeConfiguration<ActionPlanItem>
{
    public void Configure(EntityTypeBuilder<ActionPlanItem> b)
    {
        b.ToTable("ActionPlanItems");
        b.HasKey(x => x.Id);
        b.Property(x => x.Title).IsRequired();
        b.Property(x => x.Description).IsRequired();
        b.Property(x => x.DeliveryType).HasMaxLength(100);
        b.Property(x => x.ActionCatalogId).HasMaxLength(50);
        b.Property(x => x.Priority).HasConversion<string>().HasMaxLength(50).IsRequired();
        b.Property(x => x.Source).HasConversion<string>().HasMaxLength(50).IsRequired();

        b.HasIndex(x => x.ActionPlanId);
        b.HasIndex(x => x.ActionCatalogId);

        b.HasOne(x => x.ActionPlan)
            .WithMany(p => p.Items)
            .HasForeignKey(x => x.ActionPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        // string FK referencing ActionCatalog.ActionId; nullable so fallback items are allowed.
        b.HasOne(x => x.ActionCatalog)
            .WithMany(c => c.ActionPlanItems)
            .HasForeignKey(x => x.ActionCatalogId)
            .HasPrincipalKey(c => c.ActionId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.AiPredictedAction)
            .WithMany(a => a.ActionPlanItems)
            .HasForeignKey(x => x.AiPredictedActionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class EmployeeTaskConfiguration : IEntityTypeConfiguration<EmployeeTask>
{
    public void Configure(EntityTypeBuilder<EmployeeTask> b)
    {
        b.ToTable("EmployeeTasks");
        b.HasKey(x => x.Id);
        // Status persisted as string. Reads tolerate the legacy "Assigned" literal (renamed to
        // "Pending") so existing rows keep working after the rename.
        b.Property(x => x.Status)
            .HasConversion(
                v => v.ToString(),
                v => v == "Assigned"
                    ? Domain.Enums.EmployeeTaskStatus.Pending
                    : Enum.Parse<Domain.Enums.EmployeeTaskStatus>(v, ignoreCase: true))
            .HasMaxLength(50)
            .IsRequired();
        b.HasIndex(x => x.EmployeeId);

        // At most one active (non-deleted) task per action plan item — enforces send idempotency.
        b.HasIndex(x => x.ActionPlanItemId)
            .IsUnique()
            .HasDatabaseName("UX_EmployeeTasks_ActionPlanItemId_Active")
            .HasFilter("\"IsDeleted\" = false");

        b.HasOne(x => x.ActionPlanItem)
            .WithMany(i => i.EmployeeTasks)
            .HasForeignKey(x => x.ActionPlanItemId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Employee)
            .WithMany(e => e.Tasks)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.AssignedByUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> b)
    {
        b.ToTable("TaskComments");
        b.HasKey(x => x.Id);
        b.Property(x => x.CommentText).IsRequired();

        b.HasOne(x => x.Task)
            .WithMany(t => t.Comments)
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
