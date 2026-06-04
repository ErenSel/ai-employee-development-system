using BitirmeBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeBackend.Infrastructure.Persistence.Configurations;

public class ModelVersionConfiguration : IEntityTypeConfiguration<ModelVersion>
{
    public void Configure(EntityTypeBuilder<ModelVersion> b)
    {
        b.ToTable("ModelVersions");
        b.HasKey(x => x.Id);
        b.Property(x => x.ModelName).HasMaxLength(100).IsRequired();
        b.Property(x => x.Version).HasMaxLength(100).IsRequired();
    }
}

public class AiPredictionRunConfiguration : IEntityTypeConfiguration<AiPredictionRun>
{
    public void Configure(EntityTypeBuilder<AiPredictionRun> b)
    {
        b.ToTable("AiPredictionRuns");
        b.HasKey(x => x.Id);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.AssessmentId);

        b.HasOne(x => x.Assessment)
            .WithMany(a => a.PredictionRuns)
            .HasForeignKey(x => x.AssessmentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ModelVersion)
            .WithMany(m => m.PredictionRuns)
            .HasForeignKey(x => x.ModelVersionId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RequestedByUser)
            .WithMany()
            .HasForeignKey(x => x.RequestedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AiPredictedActionConfiguration : IEntityTypeConfiguration<AiPredictedAction>
{
    public void Configure(EntityTypeBuilder<AiPredictedAction> b)
    {
        b.ToTable("AiPredictedActions", t =>
            t.HasCheckConstraint("CK_AiPredictedActions_Probability", "\"Probability\" >= 0 AND \"Probability\" <= 1"));
        b.HasKey(x => x.Id);
        b.Property(x => x.ActionCode).HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.PredictionRunId);

        b.HasOne(x => x.PredictionRun)
            .WithMany(r => r.PredictedActions)
            .HasForeignKey(x => x.PredictionRunId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ActionCatalogConfiguration : IEntityTypeConfiguration<ActionCatalog>
{
    public void Configure(EntityTypeBuilder<ActionCatalog> b)
    {
        b.ToTable("ActionCatalogs", t =>
        {
            t.HasCheckConstraint("CK_ActionCatalogs_ScoreRange", "\"MinScore\" <= \"MaxScore\"");
            t.HasCheckConstraint("CK_ActionCatalogs_EffortHours", "\"EstimatedEffortHours\" >= 0");
        });

        b.HasKey(x => x.ActionId);
        b.Property(x => x.ActionId).HasMaxLength(50).IsRequired();
        b.Property(x => x.TargetCompetency).HasMaxLength(50).IsRequired();
        b.Property(x => x.ActionCategory).HasMaxLength(50).IsRequired();
        b.Property(x => x.ActionType).HasMaxLength(50).IsRequired();
        b.Property(x => x.Difficulty).HasMaxLength(20).IsRequired();
        b.Property(x => x.MinScore).HasPrecision(3, 2);
        b.Property(x => x.MaxScore).HasPrecision(3, 2);
        b.Property(x => x.ContentData).HasColumnType("jsonb").IsRequired();
    }
}
