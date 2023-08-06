using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WeedDatabase.Domain.App.Interfaces;

namespace WeedDatabase.Domain.Common;

[Table("users", Schema = "common")]
[Index(nameof(Id))]
[Index(nameof(Role))]
[Index(nameof(TelegramUserId))]
public class SmokiUser : DomainObject
{
    [Column("role")]
    public SmokiUserRole Role { get; set; }
    
    [Column("telegram_user_id")]
    public long TelegramUserId { get; set; }
}