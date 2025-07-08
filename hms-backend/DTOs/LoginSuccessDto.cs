
namespace HmsBackend.DTOs
{
    public class LoginSuccessDto
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Token { get; set; }
        public required string UserId { get; set; }
    }
}
