using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebApp03.Models.Config
{
    public class ExamConfig: EntityTypeConfiguration<Exam>
    {
        public ExamConfig()
        {
            this.ToTable("Exam");
            this.HasKey(e => e.Id);
            this.Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(e => e.Score).IsRequired();
            this.Property(e => e.EmpId).IsRequired();
            
            this.Property(e => e.SubjectId).IsRequired();
            this.HasRequired(e => e.Emp).WithMany(e => e.Exams)
                .HasForeignKey(e => e.EmpId);
            this.HasRequired(e => e.Subject).WithMany(s => s.Exams)
                .HasForeignKey(e => e.SubjectId);
        }
    }
}