using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExScheduler_Server.Models
{
    public class ExamSchedule
    {
        [Key]
        public Guid examScheduleID { get; set; }
        public string examDate { get; set; } = default!;
        public List<LinkExamDate> LinkExamDates { get; set; } = default!;
    }
}
