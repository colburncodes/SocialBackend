using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Social.Infrastructure.Data;

[Table("Accounts")]
public class Account
{
    [Key] 
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Email { get; set; }
    [Required]
    [StringLength(128)]
    public byte[] PasswordHash { get; set; }
    [Required]
    [StringLength(128)]
    public byte[] PasswordSalt { get; set; }
    public User User { get; set; }
}