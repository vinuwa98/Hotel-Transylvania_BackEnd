    using System.ComponentModel.DataAnnotations;

public class UpdateUserDto
{
    [Required]
    public string UserId { get; set; }

   
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    
    public string? Role { get; set; }

    [Phone]
    public string? ContactNumber { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DOB { get; set; }

    [MinLength(6)]
    public string? Password { get; set; }


    [EmailAddress]
    public string? Email { get; set; }

    public string? Address { get; set; }
}
