using ExScheduler_Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace ExScheduler_Server.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; } 
        public DbSet<Students> Students { get; set; }
        public DbSet<ProgrammeSemester> ProgramSemesters { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<ExamSchedule> ExamSchedules { get; set; }
        public DbSet<LinkExamDate> LinkExamDates { get; set; }
        public DbSet<LinkCourse> LinkCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LinkCourse>()
                .HasKey(lc => new { lc.LinkID, lc.CourseID });

            modelBuilder.Entity<LinkCourse>()
                .HasOne(lc => lc.Link)
                .WithMany(l => l.LinkCourses)
                .HasForeignKey(lc => lc.LinkID);

            modelBuilder.Entity<LinkCourse>()
                .HasOne(lc => lc.Course)
                .WithMany(c => c.LinkCourses)
                .HasForeignKey(lc => lc.CourseID);


            modelBuilder.Entity<LinkExamDate>()
                .HasKey(le => new { le.LinkID, le.ExamScheduleID, le.ProgrammeSemesterID, le.Priority });

            modelBuilder.Entity<LinkExamDate>()
                .HasOne(le => le.Link)
                .WithMany(l => l.LinkExamDates)
                .HasForeignKey(le => le.LinkID);

            modelBuilder.Entity<LinkExamDate>()
                .HasOne(le => le.ExamSchedule)
                .WithMany(e => e.LinkExamDates)
                .HasForeignKey(le => le.ExamScheduleID);
        }
    }
}
