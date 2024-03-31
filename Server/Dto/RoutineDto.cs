namespace ExScheduler_Server.Dto
{
    public class RoutineDto
    {
        public string ExamDate { get; set; } = default!;
        public List<GenerateExamScheduleDto> ExamSchedules { get; set; } = new List<GenerateExamScheduleDto>();
    }
}
