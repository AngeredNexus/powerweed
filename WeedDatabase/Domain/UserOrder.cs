using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Domain;


[Table("user_orders")]
public class UserOrder
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [ForeignKey("user_id")]
    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [Column("contact_id")]
    public Guid ContactId { get; set; }
    
    [Column("products_price")]
    public decimal ProductPrice { get; set; }
    
    [Column("discounted_products_price")]
    public decimal DiscountedProdcutPrice { get; set; }
    
    [Column("delivery_price")]
    public decimal DeliveryPrice { get; set; }
    
    [Column("address")]
    public string MapUrl { get; set; } = null!;
 
    [Column("created")]
    public DateTime Created { get; set; }
}