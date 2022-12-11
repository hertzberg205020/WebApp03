using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebApp03.Models.Config
{
    public class EmpConfig: EntityTypeConfiguration<Emp>
    {
        public EmpConfig()
        {
            this.ToTable("Emp");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.Name).IsRequired();
            this.Property(p => p.EmpNo).IsRequired();
            this.HasMany(e => e.Exams).WithRequired(e => e.Emp)
                .HasForeignKey(e => e.EmpId);
        }
    }
}