using System.ComponentModel.DataAnnotations;

namespace ExScheduler_Server.Models
{
    public class Students
    {
        [Key]
        public int StudentID { get; set; }
        [Required]

        public string StudentName { get; set; } = default!;
        [Required]
        [EmailAddress]
        public string StudentEmail { get; set; } = default!;
        [Required]
        public string StudentPassword { get; set; } = default!;
        [Required]
        public string Salt { get; set; } = default!;
        public bool Validity { get; set; } = false;
        public ProgrammeSemester program { get; set; } = default!;
    }
}
