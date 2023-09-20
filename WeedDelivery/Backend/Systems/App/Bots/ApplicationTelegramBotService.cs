using Telegram.Bot.Types.ReplyMarkups;
using WeedDatabase.Domain.App.Types;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Models.Api.Bots;
using WeedDelivery.Backend.Models.Telegram.Menu;
using WeedDelivery.Backend.Systems.Messangers.Interfaces;
using WeedDelivery.Backend.Systems.Messangers.Models;
using WeedDelivery.Backend.Systems.Messangers.Models.MessengerSendingMessageObject;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;

namespace WeedDelivery.Backend.Systems.App.Bots;

public class ApplicationTelegramBotService : IApplicationTelegramBotService
{

    private readonly IMessengerBotFactory _messengerBotFactory;
    private readonly ITelegramUserRepository _tgUserRepository;
    private readonly IUserRepository _userRepository;
    
    private readonly ILogger _logger;

    public ApplicationTelegramBotService(IMessengerBotFactory messengerBotFactory, IUserRepository userRepository, ILogger<ApplicationTelegramBotService> logger, ITelegramUserRepository tgUserRepository)
    {
        _messengerBotFactory = messengerBotFactory;
        _userRepository = userRepository;
        _logger = logger;
        _tgUserRepository = tgUserRepository;
    }

    public async Task NotifyAboutOrder(BotOrderNotification order, SmokiUser userData)
    {

        var tgUser = await _tgUserRepository.GetTelegramMainBotUser(Convert.ToInt64(userData.SourceIdentificator));

        var customerModule = _messengerBotFactory.GetSpecificBotInstance(MessengerSourceType.Telegram, MessengerBotType.Customer);
        var operatorModule = _messengerBotFactory.GetSpecificBotInstance(MessengerSourceType.Telegram, MessengerBotType.Operator);

        var dateStr = DateTime.Now.AddHours(4).ToString("dd-MM-yyyy");
        var timeStr = DateTime.Now.AddHours(4).ToString("HH:mm");
        
        var orderSum = order.OrderPrice;
        
        var totalItems = string.Join("; ", order.Items.Select(x => $"{x.Name} : {x.Amount}"));
        
        var customerRuMsg = "Wassup, bro! \n" +
                            $"Ваш заказ({order.Id}) от [{dateStr} -- {timeStr}] на сумму ฿{orderSum} принят! \n" + 
                            "Заказ будет доставлен в течении 40-80 минут! Изменение статуса заказа будет отображено в этом чате!";
        
        var customerEngMsg = "Wassup, bro! \n" +
                                 $"Your order({order.Id}) from [{dateStr} -- {timeStr}] for the amount of ฿{orderSum} has been accepted! \n" +
                                  "Order will be delivered in 40-80 minutes! Status change will be displayed in this chat!";

        var customerMsg = tgUser.Lang == LanguageTypes.RU ? customerRuMsg : customerEngMsg;
        
        var operatorMessage = "" +
                            $"Новый заказ ({order.Id})! \n" +
                            $"Дата: {dateStr}; Время: {timeStr}; Сумма: {orderSum}฿ \n" +
                            $"Адрес: {order.Address} \n" +
                            $"Номер: {order.PhoneNumber} \n" + 
                            $"Количество: {order.TotalAmount} \n" + 
                            $"Имя: {order.Firstname} {order.Lastname} \n" +
                            $"Комментарий: {order.Comment} \n" +
                            $"Товары: [ {totalItems} ] \n" +
                            $"Контакт: @{userData.Name}";

        var wrappedOrderForOperatorTelegramNotification = new TelegramBotOrderManageApi()
        {
            OrderId = order.Id
        };

        var operators = await _userRepository.GetOps();

        try
        {
            await customerModule.SendMessage(new MessengerDataSendObject()
            {
                AppUser = userData,
                Message = customerMsg,
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Неудалось отправить уведомление пользователю ({usrid}), ошибка: {msg}", userData.SourceIdentificator, ex.Message);
        }

        foreach (var op in operators)
        {
            try
            {
                await operatorModule.SendMessage(new MessengerDataSendObject()
                {
                    AppUser = op,
                    Message = operatorMessage,
                    MessageObject = new TelegramSendingMessage()
                    {
                        Markup = new InlineKeyboardMarkup(new []
                        {
                            InlineKeyboardButton.WithCallbackData("принят", $"/status set {order.Id} {OrderStatus.Prepare}"),
                            InlineKeyboardButton.WithCallbackData("доставка", $"/status set {order.Id} {OrderStatus.Delivery}"),
                            InlineKeyboardButton.WithCallbackData("завершен", $"/status set {order.Id} {OrderStatus.Ready}"),
                            InlineKeyboardButton.WithCallbackData("отмена", $"/status set {order.Id} {OrderStatus.Canceled}"),
                        })
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Неудалось отправить уведомление оператору ({usrid}), ошибка: {msg}", op.SourceIdentificator, ex.Message);
            }
        }
    }
}