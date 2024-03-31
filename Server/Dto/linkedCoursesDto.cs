namespace ExScheduler_Server.Dto
{
    public class linkedCoursesDto
    {
        public string LinkName { get; set; } = default!;
        public List<string> Courses { get; set;} = new List<string>();
    }
}
