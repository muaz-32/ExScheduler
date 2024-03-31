namespace ExScheduler_Server.Dto
{
    public class StudentDto
    {
        public int StudentId { get; set; } = default!;
        public string StudentName { get; set; } = default!;
        public string StudentEmail { get; set; } = default!;
        public string programeSemester { get; set; } = default!;    
        public string StudentPassword { get; set; } = default!;
        public string StudentConfirmPassword { get; set; } = default!;
        public string salt { get; set; } = default!;

    }
}
