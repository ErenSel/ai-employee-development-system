using BitirmeBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeBackend.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> b)
    {
        b.ToTable("Departments");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.Code).HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.Code).IsUnique();
    }
}

public class JobRoleConfiguration : IEntityTypeConfiguration<JobRole>
{
    public void Configure(EntityTypeBuilder<JobRole> b)
    {
        b.ToTable("JobRoles");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.HasIndex(x => x.DepartmentId);

        b.HasOne(x => x.Department)
            .WithMany(d => d.JobRoles)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> b)
    {
        b.ToTable("Employees", t =>
            t.HasCheckConstraint("CK_Employees_Age", "\"Age\" > 0"));
        b.HasKey(x => x.Id);
        b.Property(x => x.EmployeeCode).HasMaxLength(50).IsRequired();
        b.Property(x => x.FullName).HasMaxLength(150).IsRequired();
        b.Property(x => x.Email).HasMaxLength(150).IsRequired();
        b.Property(x => x.Gender).HasMaxLength(30).IsRequired();
        b.Property(x => x.Education).HasMaxLength(50).IsRequired();
        b.Property(x => x.EducationField).HasMaxLength(100).IsRequired();
        b.Property(x => x.BusinessTravel).HasMaxLength(100).IsRequired();
        b.Property(x => x.MaritalStatus).HasMaxLength(50).IsRequired();
        b.Property(x => x.Attrition).HasMaxLength(10).IsRequired();

        b.HasIndex(x => x.EmployeeCode).IsUnique();
        b.HasIndex(x => x.Email).IsUnique();
        b.HasIndex(x => x.DepartmentId);
        b.HasIndex(x => x.JobRoleId);
        b.HasIndex(x => x.ManagerId);

        b.HasOne(x => x.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.JobRole)
            .WithMany(j => j.Employees)
            .HasForeignKey(x => x.JobRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Manager)
            .WithMany(e => e.DirectReports)
            .HasForeignKey(x => x.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
