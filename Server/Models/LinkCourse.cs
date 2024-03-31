namespace ExScheduler_Server.Models
{
    public class LinkCourse
    {
        public Guid LinkID { get; set; }
        public Links Link { get; set; } = default!;
        public Guid CourseID { get; set; }
        public Course Course { get; set; } = default!;
    }
}
