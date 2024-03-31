using System.ComponentModel.DataAnnotations;

namespace ExScheduler_Server.Models
{
    public class ProgrammeSemester
    {
        [Key]
        public Guid programSemesterID { get; set; }
        public string programSemesterName { get; set; } = default!;
        public List<Students> Students { get; set; } = default!;   
        public Department department { get; set; } = default!;
        public List<Course> courses { get; set; } = default!;
    }
}
