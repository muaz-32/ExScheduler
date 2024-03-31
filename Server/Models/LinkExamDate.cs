namespace ExScheduler_Server.Models
{
    public class LinkExamDate
    {
        public Guid LinkID { get; set; }
        public Links Link { get; set; } = default!;
        public Guid ExamScheduleID { get; set; }
        public ExamSchedule ExamSchedule { get; set; } = default!;
        public Guid ProgrammeSemesterID { get; set; }
        public ProgrammeSemester ProgrammeSemester { get; set; } = default!;
        public int Priority { get; set; } = 0;
    }
}
