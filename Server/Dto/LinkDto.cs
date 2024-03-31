using ExScheduler_Server.Models;
using Org.BouncyCastle.Asn1.Mozilla;

namespace ExScheduler_Server.Dto
{
    public class LinkDto
    {
        public string linkname { get; set; } = default!; 
        public List<string> courses { get; set; } = default!;
    }
}
