using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WeedDatabase.Domain.App.Interfaces;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;

namespace WeedDatabase.Domain.Telegram;

[Table("bot_user_map", Schema = "telegram")]
[Index(nameof(Id))]
[Index(nameof(Hash), IsUnique = true)]
[Index(nameof(UserId))]
public class TelegramBotUser : DomainObject
{
    [Column("hash")]
    public string Hash { get; set; } = null!;

    [Column("app_user_id")]
    public Guid AppUserId { get; set; }
    
    [Column("messenger")]
    public MessengerSourceType MessengerSource { get; set; }
    
    [Column("user_id")]    
    public long UserId { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; }
    
    [Column("lang")]
    public LanguageTypes Lang { get; set; }
    
    [Column("has_accepted_law_policy")]
    public bool HasAcceptedLawPolicy { get; set; }
}