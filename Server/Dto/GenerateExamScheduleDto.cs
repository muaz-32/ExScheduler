namespace ExScheduler_Server.Dto
{
    public class GenerateExamScheduleDto
    {
        public string ProgramSemesterName { get; set; } = default!;
        public GenerateCourseDto Course { get; set; } = new GenerateCourseDto();
    }
}
