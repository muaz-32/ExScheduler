using System.ComponentModel.DataAnnotations;

namespace ExScheduler_Server.Models
{
    public class Links
    {
        [Key]
        public Guid linkID { get; set; }
        public string linkname { get; set; } = default!;
        public List<Course> courses { get; set; } = default!;
        public List<ExamSchedule> examSchedules { get; set; } = default!;
        public List<LinkExamDate> LinkExamDates { get; set; } = default!;
        public List<LinkCourse> LinkCourses { get; set; } = default!;
    }
}
