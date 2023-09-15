using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WeedDatabase.Domain.App.Interfaces;

namespace WeedDatabase.Domain.Common;

[Table("users", Schema = "common")]
[Index(nameof(Id))]
[Index(nameof(Role))]
[Index(nameof(SourceIdentificator))]
public class SmokiUser : DomainObject
{
    [Column("name")] 
    public string? Name { get; set; }

    [Column("role")] 
    public SmokiUserRole Role { get; set; }

    [Column("source")]
    public IdentitySource Source { get; set; }

    [Column("source_identificator")] 
    public string SourceIdentificator { get; set; } = string.Empty;

    [Column("identity_hash")] 
    public string IdentityHash { get; set; } = string.Empty;
}