using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ExamMonitoringWeb.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<ExamLab> ExamLabs { get; set; }
        public DbSet<StudentExam> StudentExams { get; set; }
        public DbSet<Violation> Violations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExamLab>()
                .HasKey(x => new { x.ExamId, x.LabId });

            builder.Entity<ExamLab>()
                .HasOne(x => x.Exam)
                .WithMany(e => e.ExamLabs)
                .HasForeignKey(x => x.ExamId);

            builder.Entity<ExamLab>()
                .HasOne(x => x.Lab)
                .WithMany(l => l.ExamLabs)
                .HasForeignKey(x => x.LabId);

            builder.Entity<StudentExam>()
                .HasKey(x => new { x.UserId, x.ExamId });

            builder.Entity<StudentExam>()
                .HasOne(x => x.User)
                .WithMany(u => u.StudentExams)
                .HasForeignKey(x => x.UserId);

            builder.Entity<StudentExam>()
                .HasOne(x => x.Exam)
                .WithMany(e => e.StudentExams)
                .HasForeignKey(x => x.ExamId);

            builder.Entity<Violation>()
                .HasOne(v => v.User)
                .WithMany(u => u.Violations)
                .HasForeignKey(v => v.UserId);

            builder.Entity<Violation>()
                .HasOne(v => v.Exam)
                .WithMany(e => e.Violations)
                .HasForeignKey(v => v.ExamId);
        }
    }
}
