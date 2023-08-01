using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WeedDatabase.Domain.App.Interfaces;
using WeedDatabase.Domain.App.Types;

namespace WeedDatabase.Domain.App;

[Table("orders", Schema = "store")]
[Index(nameof(Id))]
[Index(nameof(PhoneNumber))]
public class Order : DomainObject
{
    [Column("firstname")]
    public string Firstname { get; set; }
    
    [Column("lastname")]
    public string Lastname { get; set; }
    
    [Column("phone_number")]
    public string PhoneNumber { get; set; }
    
    [Column("address")]
    public string Address { get; set; }
    
    [Column("delivery")]
    public string? DeliveryMan { get; set; }
    
    [Column("status")]
    public OrderStatus Status { get; set; }

    [Column("item")] 
    public List<OrderItem> Items { get; set; } = new();
}