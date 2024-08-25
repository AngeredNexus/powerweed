using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Domain;

[Table("auth_contact")]
public class AuthContact
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("user_id")]
    [ForeignKey("user_id")]
    public Guid UserId { get; set; }
    
    [Column("type")]
    public ContactType Type { get; set; }
    
    [Column("dataId")]
    public Guid DataId { get; set; }
}