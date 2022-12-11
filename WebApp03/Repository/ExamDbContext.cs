using System.Data.Entity;
using System.Reflection;
using WebApp03.Models;

namespace WebApp03.Repository
{
    public class ExamDbContext: DbContext
    {
        public ExamDbContext(): base("name=ExamDb") {
            // 禁用使用code反向建表
            Database.SetInitializer<ExamDbContext>(null);
        }
        
        // 寫實體集
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Emp> Emps { get; set; }
        public DbSet<EmpScore> EmpScores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 方案一   
            // 從某個程序集加載所有繼承自EntityTypeConfiguration的類別到配置中
            modelBuilder.Configurations.AddFromAssembly(
                Assembly.GetExecutingAssembly()
            );
        }
    }
}