using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WeedDatabase.Domain.App.Interfaces;
using WeedDatabase.Domain.App.Types;

namespace WeedDatabase.Domain.App;

[Table("weed", Schema = "store")]
[Index(nameof(Id))]
[Index(nameof(Unique))]
[Index(nameof(Name))]
public class WeedItem : DomainObject
{
    [Column("unqiue")]
    public string Unique { get; set; }

    [Column("name")]
    public string Name { get; set; }
    
    [Column("descriptions")]
    public string Description { get; set; }
    
    [Column("photo_url")]
    public string PhotoUrl { get; set; }
    
    [Column("strain")]
    public StrainType StrainType { get; set; }
    
    // public TripType TripType { get; set; }
    //
    // public PowerType PowerType { get; set; }
    //
    // public DailyType DailyType { get; set; }
    
    
    [Column("thc")]
    [Range(0, 100)]
    public Decimal Thc { get; set; }
    
    [Column("price")]
    [Range(300, 10000)]
    public int Price { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal PhysicalRelaxation { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal MindRelaxation { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal Euphoretic { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal Happpiness { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal Audiophile { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal Videophile { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal Moviephile { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal Communicative { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal Creative { get; set; }
    //
    // [Range(0, 100)]
    // public Decimal Romantic { get; set; }
}