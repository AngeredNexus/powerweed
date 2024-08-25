using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Domain;

[Table("product_specifications")]
public class ProductSpecification
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("product_id")]
    [ForeignKey("product_id")]
    public Guid ProductId { get; set; }
    
    [Column("thc")]
    public string THC { get; set; } = "?";
    
    [Column("tgk")]
    public string TGK { get; set; } = "?";
    
    [Column("strain")]
    public string Strain { get; set; } = "?";
    
    [Column("effekt")]
    public string Effekt { get; set; } = "?";
    
    [Column("time")]
    public string Time { get; set; } = "?";
    
}