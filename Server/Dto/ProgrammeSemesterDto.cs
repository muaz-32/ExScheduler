namespace ExScheduler_Server.Dto
{
    public class ProgrammeSemesterDto
    {
        public string programmeSemesterName { get; set; } = default!;
        public string departmentName { get; set; } = default!; 
        public Guid departmentId { get; set;} = default!;
    }
}
