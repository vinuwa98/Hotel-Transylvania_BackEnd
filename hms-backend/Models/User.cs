using Microsoft.AspNetCore.Identity;

namespace HmsBackend.Models
{
    public class User : IdentityUser
    {
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Password { get; set; }
    }
}
