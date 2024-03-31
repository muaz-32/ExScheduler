namespace ExScheduler_Server.Dto
{
    public class LinkCourseDatePriorityDto
    {
        public string LinkName { get; set; }=default!;
        public string CourseName { get; set; } = default!;
        public string ExamDate { get; set; } = default!;
        public int Priority { get; set; }=default!;
    }
}
