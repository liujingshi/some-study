using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace permission.Entities;

[Table("users")]
public class User
{
    [Key]
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}
