namespace ExScheduler_Server.Dto
{
    public class LinkDatesDto
    {
        public string linkName { get; set; } = default!;
        public string examDate { get; set; } = default!;  
        public string programmeSemester { get; set; } = default!;  
        public int priority { get; set; } = 0;
    }
}
