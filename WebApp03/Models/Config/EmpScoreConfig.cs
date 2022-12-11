using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebApp03.Models.Config;

public class EmpScoreConfig: EntityTypeConfiguration<EmpScore>
{
    public EmpScoreConfig()
    {
        this.ToTable("t_emp_score");
        this.HasKey(e => e.Id);
        this.Property(e => e.Id)
            .HasColumnName("id")
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        
        this.Property(e => e.Name).HasColumnName("name").IsRequired();
        // this.Property(e => e.Name).HasColumnName("name");
        this.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        this.Property(e => e.Course1).HasColumnName("course1");
        this.Property(e => e.Course2).HasColumnName("course2");
        this.Property(e => e.Course3).HasColumnName("course3");
    }
}