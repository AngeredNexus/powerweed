using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Domain;

[Table("telegram_contacts")]
public class TelegramContact
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [ForeignKey("contact_id")]
    [Column("contact_id")]
    public Guid ContactId { get; set; }
    
    [Column("chat_id")]
    public string ChatId { get; set; } = null!;
    
    [Column("user")]
    public string Username { get; set; } = null!;
}