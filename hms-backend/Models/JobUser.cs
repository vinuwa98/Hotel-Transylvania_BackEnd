using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HmsBackend.Models
{
    public class JobUser
    {
        [Key, Column(Order = 0)]
        public int JobId { get; set; }

        public Job Job { get; set; }

        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
