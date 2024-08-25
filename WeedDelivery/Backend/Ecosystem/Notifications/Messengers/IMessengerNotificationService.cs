using Database.Domain;
using WeedDelivery.Backend.Ecosystem.Store;
using WeedDelivery.Backend.Models.Api.Request;

namespace WeedDelivery.Backend.Ecosystem.Notifications.Messengers;

public interface IMessengerNotificationService
{
    Task SendOrderNotification(NotificationRole role, UserOrder order, OrderRequest request, AuthContact contact,
        List<ProductWithOrderInfo> productInfo);
}