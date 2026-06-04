using BitirmeBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeBackend.Infrastructure.Persistence.Configurations;

public class CompetencyConfiguration : IEntityTypeConfiguration<Competency>
{
    public void Configure(EntityTypeBuilder<Competency> b)
    {
        b.ToTable("Competencies");
        b.HasKey(x => x.Id);
        b.Property(x => x.Code).HasMaxLength(100).IsRequired();
        b.Property(x => x.Name).HasMaxLength(150).IsRequired();
        b.Property(x => x.Category).HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.Code).IsUnique();
    }
}

public class AssessmentCycleConfiguration : IEntityTypeConfiguration<AssessmentCycle>
{
    public void Configure(EntityTypeBuilder<AssessmentCycle> b)
    {
        b.ToTable("AssessmentCycles");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(150).IsRequired();
        b.Property(x => x.StartDate).HasColumnType("date");
        b.Property(x => x.EndDate).HasColumnType("date");
        b.Property(x => x.Status).HasMaxLength(50).IsRequired();
    }
}

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> b)
    {
        b.ToTable("Assessments");
        b.HasKey(x => x.Id);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.EmployeeId);

        b.HasOne(x => x.Employee)
            .WithMany(e => e.Assessments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Cycle)
            .WithMany(c => c.Assessments)
            .HasForeignKey(x => x.CycleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AssessmentScoreConfiguration : IEntityTypeConfiguration<AssessmentScore>
{
    public void Configure(EntityTypeBuilder<AssessmentScore> b)
    {
        b.ToTable("AssessmentScores", t =>
            t.HasCheckConstraint("CK_AssessmentScores_Score", "\"Score\" >= 0 AND \"Score\" <= 5"));
        b.HasKey(x => x.Id);
        b.Property(x => x.EvaluatorType).HasConversion<string>().HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.AssessmentId);
        b.HasIndex(x => x.CompetencyId);

        b.HasOne(x => x.Assessment)
            .WithMany(a => a.Scores)
            .HasForeignKey(x => x.AssessmentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Competency)
            .WithMany(c => c.AssessmentScores)
            .HasForeignKey(x => x.CompetencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class FeedbackCommentConfiguration : IEntityTypeConfiguration<FeedbackComment>
{
    public void Configure(EntityTypeBuilder<FeedbackComment> b)
    {
        b.ToTable("FeedbackComments");
        b.HasKey(x => x.Id);
        b.Property(x => x.EvaluatorType).HasConversion<string>().HasMaxLength(50).IsRequired();
        b.Property(x => x.CommentText).IsRequired();

        b.HasOne(x => x.Assessment)
            .WithMany(a => a.FeedbackComments)
            .HasForeignKey(x => x.AssessmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
