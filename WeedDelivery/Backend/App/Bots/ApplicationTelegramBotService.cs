using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Bots.Telegram.Common.Interfaces;
using WeedDelivery.Backend.Models.Api.Bots;
using WeedDelivery.Backend.Models.Api.Common;
using WeedDelivery.Backend.Models.Telegram.Menu;

namespace WeedDelivery.Backend.App.Bots;

public class ApplicationTelegramBotService : IApplicationTelegramBotService
{

    private readonly ITelegramBotFactory _telegramBotFactory;
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;

    public ApplicationTelegramBotService(ITelegramBotFactory telegramBotFactory, IUserRepository userRepository, ILogger<ApplicationTelegramBotService> logger)
    {
        _telegramBotFactory = telegramBotFactory;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task NotifyAboutOrder(BotOrderNotification order, TelegramCoockie userData)
    {

        var operatorModule = _telegramBotFactory.GetModule(TelegramBotType.OrderNotificationOperatorBot);
        var customerModule = _telegramBotFactory.GetModule(TelegramBotType.OrderNotificationCustomerBot);

        var dateStr = DateTime.Now.ToString("dd-MM-yyyy");
        var timeStr = DateTime.Now.ToString("HH:mm");
        var sum = order.TotalSum + order.TotalSumWithDelivery;
        
        
        var totalItems = string.Join("; \n", order.Items.Select(x => $"{x.Name} : {x.Amount}"));
        
        var customerRuMsg = "Wassup, bro! \n" +
                            $"Ваш заказ от [{dateStr} -- {timeStr}] на сумму ฿{sum} принят! \n" + 
                            "Заказ будет доставлен в течении 1.5 часов; Изменение статуса заказа будет отображено в этом чате!";
        
        var customerEngMsg = "Wassup, bro! \n" +
                                 $"Your order from [{dateStr} -- {timeStr}] for the amount of ฿{sum} has been accepted! \n" +
                                  "Order will be delivered in 1.5h; Status change will be displayed in this chat!";
        
        var operatorMessage = "" +
                            $"Новый заказ! " +
                            $"Дата: {dateStr}; Время: {timeStr}; Сумма: {sum}฿ \n" +
                            $"Адрес: {order.Address} \n" +
                            $"Номер: {order.PhoneNumber} \n" + 
                            $"Количество: {order.TotalAmount} \n" + 
                            $"Имя: {order.Firstname} {order.Lastname} \n" +
                            $"Комментарий: {order.Comment} \n" +
                            $"Товары: [{totalItems}] \n" +
                            @"< ушел в доставку; доставлен; отмена; возобновить >";

        var wrappedOrderForOperatorTelegramNotification = new TelegramBotOrderManageApi()
        {
            OrderId = order.Id
        };

        var operators = await _userRepository.GetOps();

        try
        {
            if (userData.Language is LanguageTypes.EN)
                await customerModule.SendMessageAsync($"{userData.Id}", customerEngMsg);
            else
                await customerModule.SendMessageAsync($"{userData.Id}", customerRuMsg);

        }
        catch (Exception ex)
        {
            _logger.LogWarning("Неудалось отправить уведомление пользователю ({usrid}), ошибка: {msg}", userData.Id, ex.Message);
        }

        foreach (var op in operators)
        {
            try
            {
                await operatorModule.SendMessageAsync(op.SourceIdentificator,
                    operatorMessage); //, wrappedOrderForOperatorTelegramNotification);    
            }
            catch (Exception ex)
            {
                _logger.LogError("Неудалось отправить уведомление оператору ({usrid}), ошибка: {msg}", op.SourceIdentificator, ex.Message);
            }
        }
    }
}