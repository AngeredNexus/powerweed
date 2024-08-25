using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Domain;

[Table("user")]
public class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [ForeignKey("contact_id")]
    [Column("contact_id")]
    public Guid? ContactId { get; set; }
    
    [Column("roles")]
    public string[] Roles { get; set; }
    
    [Column("token")]
    public string TokenSync { get; set; } = string.Empty;
}