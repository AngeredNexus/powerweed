using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Domain;

[Table("product_image")]
public class ProductImage
{
    [Column("id")]
    public Guid Id {get;set;}
    
    [Column("product_id")]
    public Guid ProductId {get; set;}
    
    [Column("value")]
    public string Value { get; set; } = null!;
}