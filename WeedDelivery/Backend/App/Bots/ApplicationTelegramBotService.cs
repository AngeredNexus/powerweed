using WeedDatabase.Domain.App;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;
using WeedDelivery.Backend.Bots.Telegram.Common.Interfaces;
using WeedDelivery.Backend.Models.Api.Bots;
using WeedDelivery.Backend.Models.Api.Common;

namespace WeedDelivery.Backend.App.Bots;

public class ApplicationTelegramBotService : IApplicationTelegramBotService
{

    private readonly ITelegramBotFactory _telegramBotFactory;

    public ApplicationTelegramBotService(ITelegramBotFactory telegramBotFactory)
    {
        _telegramBotFactory = telegramBotFactory;
    }

    public async Task NotifyAboutOrder(BotOrderNotification order, TelegramCoockie userData)
    {

        var operatorModule = _telegramBotFactory.GetModule(TelegramBotType.OrderNotificationOperatorBot);
        var customerModule = _telegramBotFactory.GetModule(TelegramBotType.OrderNotificationCustomerBot);

        var dateStr = DateTime.Now.ToString("dd-MM-yyyy");
        var timeStr = DateTime.Now.ToString("HH:mm");
        var sum = order.TotalSum + order.TotalSumWithDelivery;
        
        
        var customerRuMsg = $@"
                            Ваш заказ от {dateStr} -- {timeStr} на сумму {sum} принят! 
                            Изменение статуса заказа будет отображено в этом чате!
                            ";
        
        var customerEngMsg = $@"
                            Your order from {dateStr} -- {timeStr} na summu {sum} in progress! 
                            Changing order's status will displayd here!
                            ";

        var totalItems = string.Join("; ", order.Items.Select(x => $"{x.Name} : {x.Amount}"));
        
        var operatorMessage = $@"
                            Новый заказ ({dateStr}, {timeStr}, {sum})!
                            Адрес: {order.Address}
                            Количество: {order.TotalAmount}
                            Товары: [{totalItems}]
                            <ушел в доставку; доставлен; отмена; возобновить>
                            ";

        if(userData.Language is LanguageTypes.EN)
            await customerModule.SendMessageAsync($"{userData.Id}", customerEngMsg);
        else
            await customerModule.SendMessageAsync($"{userData.Id}", customerRuMsg);
        
        await operatorModule.SendMessageAsync($"{userData.Id}", operatorMessage);
        
    }
}