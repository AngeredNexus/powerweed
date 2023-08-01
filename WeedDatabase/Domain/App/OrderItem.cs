using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WeedDatabase.Domain.App.Interfaces;

namespace WeedDatabase.Domain.App;

[Table("orders", Schema = "items")]
[Index(nameof(Id))]
[Index(nameof(OrderId))]
[Index(nameof(WeedId))]
public class OrderItem : DomainObject
{
    
    [Column("order_id")]
    public Guid OrderId { get; set; }
    
    [Column("weed_id")]
    public Guid WeedId { get; set; }
    
    [Column("amount")]
    public int Amount { get; set; }
    
    /// <summary>
    /// Дублирующее имя для удобной отладки в БД и отображений где угодно в заказе
    /// </summary>
    [Column("name")]
    public string Name{ get; set; }
    
}