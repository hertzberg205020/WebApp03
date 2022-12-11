using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebApp03.Models.Config
{
    public class SubjectConfig: EntityTypeConfiguration<Subject>
    {
        public SubjectConfig()
        {
            this.ToTable("Subject");
            this.HasKey(s => s.Id);
            this.Property(s => s.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(s => s.Name).IsRequired();
            this.HasMany(s => s.Exams).WithRequired(e => e.Subject)
                .HasForeignKey(e => e.SubjectId);
        }
    }
}