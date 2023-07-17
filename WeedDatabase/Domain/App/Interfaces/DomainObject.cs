using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeedDatabase.Domain.App.Interfaces;

public class DomainObject : IDomainObject
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("created")]
    public DateTime Created { get; set; }
    
    [Column("modified")]
    public DateTime Modified { get; set; }
}