using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Social.Infrastructure.Data;
[Table("User")]
public class Person
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string UserName { get; set; }
    [StringLength(200)]
    public string?  Bio { get; set; }
    [Required]
    [StringLength(200)]
    public string Image { get; set; }

    public Account Account { get; set; }
}