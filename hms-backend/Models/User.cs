namespace HmsBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName {  get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
    }
}
