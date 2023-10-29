using System.ComponentModel.DataAnnotations;

namespace Social.Core.Requests;

public class Login
{
    [Required]
    [EmailAddress(ErrorMessage = "Email address must be valid.")]
    [StringLength(50, ErrorMessage = "User email max length is 50 characters.")]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}