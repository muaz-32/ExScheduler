using System.ComponentModel.DataAnnotations;

namespace ExScheduler_Server.Models
{
    public class Course
    {
        [Key]
        public Guid courseID { get; set; }
        public string courseName { get; set; } = default!;
        public ExamSchedule? ExamSchedule { get; set; }
        public ProgrammeSemester ProgrammeSemester { get; set; } = default!;
        public List<LinkCourse> LinkCourses { get; set; } = default!;
    }
}
