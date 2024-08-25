using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Domain;

[Table("product_order")]
public class ProductOrder
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [ForeignKey("order_id")]
    [Column("order_id")]
    public Guid OrderId { get; set; }
    
    [Column("product_id")]
    public Guid ProductId { get; set; }
    
    [Column("amount")]
    public int Amount { get; set; }
}