using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ExScheduler_Server.Models
{
    public class Admin
    {
        [Key]
        public Guid AdminID { get; set; }
        [Required]
        public string AdminName { get; set; } = default!;
        [Required]
        [EmailAddress]
        public string AdminEmail { get; set; } = default!;
        [Required]
        public string AdminPassword { get; set; } = default!;
        [Required]
        public string Salt { get; set; } = default!;
    }
}
