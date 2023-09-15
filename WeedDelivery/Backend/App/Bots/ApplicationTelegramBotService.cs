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
    private readonly ITelegramUserRepository _tgUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMarketAdminItemsRepository _adminItemsRepository;
    
    private readonly ILogger _logger;

    public ApplicationTelegramBotService(ITelegramBotFactory telegramBotFactory, IUserRepository userRepository, ILogger<ApplicationTelegramBotService> logger, ITelegramUserRepository tgUserRepository, IMarketAdminItemsRepository adminItemsRepository)
    {
        _telegramBotFactory = telegramBotFactory;
        _userRepository = userRepository;
        _logger = logger;
        _tgUserRepository = tgUserRepository;
        _adminItemsRepository = adminItemsRepository;
    }

    public async Task NotifyAboutOrder(BotOrderNotification order, SmokiUser userData)
    {

        var tgUser = await _tgUserRepository.GetTelegramMainBotUser(Convert.ToInt64(userData.SourceIdentificator));
        
        var operatorModule = _telegramBotFactory.GetModule(TelegramBotType.OrderNotificationOperatorBot);
        var customerModule = _telegramBotFactory.GetModule(TelegramBotType.OrderNotificationCustomerBot);
        
        var mainModule = _telegramBotFactory.GetModule(TelegramBotType.MainBot);

        var dateStr = DateTime.Now.AddHours(4).ToString("dd-MM-yyyy");
        var timeStr = DateTime.Now.AddHours(4).ToString("HH:mm");
        
        var orderSum = order.OrderPrice + order.OrderDeliveryPrice;
        
        var totalItems = string.Join("; \n", order.Items.Select(x => $"{x.Name} : {x.Amount}"));
        
        var customerRuMsg = "Wassup, bro! \n" +
                            $"Ваш заказ от [{dateStr} -- {timeStr}] на сумму ฿{orderSum} принят! \n" + 
                            "Заказ будет доставлен в течении 60-90 минут! Изменение статуса заказа будет отображено в этом чате!";
        
        var customerEngMsg = "Wassup, bro! \n" +
                                 $"Your order from [{dateStr} -- {timeStr}] for the amount of ฿{orderSum} has been accepted! \n" +
                                  "Order will be delivered in 60-90 minutes! Status change will be displayed in this chat!";

        var customerMsg = tgUser.Lang == LanguageTypes.RU ? customerRuMsg : customerEngMsg;
        
        var operatorMessage = "" +
                            $"Новый заказ! " +
                            $"Дата: {dateStr}; Время: {timeStr}; Сумма: {orderSum}฿ \n" +
                            $"Адрес: {order.Address} \n" +
                            $"Номер: {order.PhoneNumber} \n" + 
                            $"Количество: {order.TotalAmount} \n" + 
                            $"Имя: {order.Firstname} {order.Lastname} \n" +
                            $"Комментарий: {order.Comment} \n" +
                            $"Товары: [{totalItems}] \n" +
                            $"Контакт: @{userData.Name}";

        var wrappedOrderForOperatorTelegramNotification = new TelegramBotOrderManageApi()
        {
            OrderId = order.Id
        };

        var operators = await _userRepository.GetOps();

        try
        {
            await customerModule.SendMessageAsync(userData.SourceIdentificator, customerMsg);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Неудалось отправить уведомление пользователю ({usrid}), ошибка: {msg}", userData.SourceIdentificator, ex.Message);

            try
            {
                var addNotificationBotMessage = tgUser.Lang == LanguageTypes.RU ? "\nДобавьте бота уведомлений(кнопка \"Заказы\"!" : "Add notification bot(\"Orders\" button!";
                
                await mainModule.SendMessageAsync(userData.SourceIdentificator, customerMsg + addNotificationBotMessage);
            }
            catch (Exception exin)
            {
                _logger.LogWarning("Неудалось отправить уведомление пользователю в главный чат ({usrid}), ошибка: {msg}", userData.SourceIdentificator, exin.Message);
            }
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