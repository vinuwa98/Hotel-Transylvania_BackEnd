namespace hms_backend.DTOs
{
    public class LoginSuccessDto
    {
        public string? Token { get; set; }
        public required string UserId { get; set; }
    }
}
