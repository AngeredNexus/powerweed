using Database.Domain;
using Database.Repositories;
using WeedDelivery.Backend.Ecosystem.MessengerListeners;
using WeedDelivery.Backend.Ecosystem.Store;
using WeedDelivery.Backend.Models.Api.Request;

namespace WeedDelivery.Backend.Ecosystem.Notifications.Messengers;

public class MessengerNotificationService : IMessengerNotificationService
{
    private readonly IMessengerDataRepository _msgDataRepository;
    private readonly IMessengerBotService _telegramBot;
    private readonly IMessengerBotService _telegramBotManager;
    
    public MessengerNotificationService(IMultipleDependenciesResolver resolver, IMessengerDataRepository msgDataRepository)
    {
        _msgDataRepository = msgDataRepository;
        _telegramBot = resolver.Resolve(ContactType.Telegram);
        _telegramBotManager = resolver.Resolve(ContactType.TelegramManager);
    }
    
    public async Task SendOrderNotification(NotificationRole role, UserOrder order, OrderRequest request, AuthContact contact, List<ProductWithOrderInfo> productInfo)
    {
        var userInfo = $"Name: {request.Name}\n" +
                       $"Phone: {request.Phone}\n" +
                       $"Address: {request.Address}\n" +
                       $"Comment: {request.Comment}\n";

        var items = productInfo.Select(x => $"{x.ProductInfo.Name}: {x.OrderInfo.Amount} Gr.").Join("\n");
        
        var orderInfo = $"Price: {order.DiscountedProdcutPrice + order.DeliveryPrice}\u0e3f;\n" +
                             $"Product: {order.DiscountedProdcutPrice}\u0e3f; Delivery - {order.DeliveryPrice}\u0e3f\n" +
                             $"Items: \n{items}\n";

        var msg = $"ORDER ACCEPTED! {DateTime.Now:dd/MM/yyyy hh:mm:ss}\n" +
                  $"Customer info: \n{userInfo}\n\n" +
                  $"Order info: \n{orderInfo}\n\n";
        
        if (role == NotificationRole.Manager)
        {
            var tgData = await _msgDataRepository.GetContactByIdAsync(contact.DataId);
            msg += $"Contact info: @{tgData?.Username}";
            
            var managers = await _msgDataRepository.GetTelegramManagersDataIdsAsync();

            foreach (var managerChat in managers)
            {
                await _telegramBotManager.SendMessageAsync(managerChat, msg);
            }
            return;
        }
        
        await _telegramBot.SendMessageAsync(contact.DataId, msg);
    }
}