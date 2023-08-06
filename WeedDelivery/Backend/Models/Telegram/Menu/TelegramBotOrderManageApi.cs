using WeedDatabase.Domain.App.Types;

namespace WeedDelivery.Backend.Models.Telegram.Menu;

public class TelegramBotOrderManageApi
{
    public OrderStatus Action { get; set; }
    public Guid OrderId { get; set; }
}