using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using WeedDatabase.Domain.App.Types;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Models.Telegram;
using WeedDelivery.Backend.Models.Telegram.Menu;
using WeedDelivery.Backend.Models.Telegram.Menu.Common;
using WeedDelivery.Backend.Systems.App.Common;
using WeedDelivery.Backend.Systems.Messangers.Interfaces;
using WeedDelivery.Backend.Systems.Messangers.Models;
using WeedDelivery.Backend.Systems.Messangers.Models.MessengerSendingMessageObject;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;
using WeedDelivery.Backend.Systems.Messangers.Services.HostedMessengerWorker;

namespace WeedDelivery.Backend.Systems.Messangers.Services;

public class TelegramBotOperatorModule : TelegramBotApiService
{
    
    public override MessengerBotType BotType => MessengerBotType.Operator;
    
    readonly ITelegramUserRepository _telegramUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMarketAdminOrderRepository _adminOrderRepository;
    private readonly ILogger _logger;
    
    public TelegramBotOperatorModule(ILogger<TelegramBotOperatorModule> logger, ITelegramUserRepository telegramUserRepository, IUserRepository userRepository, IMarketAdminOrderRepository adminOrderRepository) : base(logger, telegramUserRepository, userRepository)
    {
        _telegramUserRepository = telegramUserRepository;
        _userRepository = userRepository;
        _adminOrderRepository = adminOrderRepository;
        _logger = logger;
    }
    
    public override async Task<MessengerDataSendObject> HandleTelegramMessegeInput(TelegramHandleRequestForm form)
    {
        if (form.AppMessage.AppUser.IsEmpty)
            throw new ArgumentException("Empty application user cannot be handled by messenger server");

        if (string.IsNullOrWhiteSpace(form.AppMessage.Message))
            return new MessengerDataSendObject();

        if (form.AppMessage.Message.StartsWith('/'))
        {
            
            var commandParts = form.AppMessage.Message.Split(' ');
            var commandStr = commandParts.First();
            var arguments = commandParts.Skip(1).ToList();
            
            var command = commandStr.GetEnumValueByUniqueName<MessengerCommand>();

            var response = command switch
            {
                MessengerCommand.Status => await HandleStatus(form, command, arguments),
            
                _ => throw new ArgumentException("Unknown command")
            };
        
            return response;
        }
        
        return new MessengerDataSendObject();
    }


    private async Task<MessengerDataSendObject> HandleStatus(TelegramHandleRequestForm form, MessengerCommand command, IReadOnlyCollection<string> arguments)
    {
        var response = new MessengerDataSendObject()
        {
            Message = string.Empty,
            MessageObject = null
        };

        if (form.User is null)
            return response;

        var subcmd = arguments.First();

        if (subcmd == "set")
        {
            try
            {
                var orderId = Guid.Parse(arguments.Skip(1).First());
                var status = Enum.Parse<OrderStatus>(arguments.Skip(2).First());

                if (orderId == Guid.Empty || status is OrderStatus.Unknown)
                    return response;

                var order = await _adminOrderRepository.GetOrderById(orderId);

                if (order is null)
                    return response;

                var userId = order.AppUserId;
                await _adminOrderRepository.SetStatus(orderId, status);

                var statusRu = status switch
                {
                    OrderStatus.Unknown => "неизвестно",
                    OrderStatus.Pending => "ожидает",
                    OrderStatus.Prepare => "подготавливается",
                    OrderStatus.Delivery => "доставка",
                    OrderStatus.Ready => "завершен",
                    OrderStatus.Canceled => "отменен"
                };
                
                var statusEn = status switch
                {
                    OrderStatus.Unknown => "unknown",
                    OrderStatus.Pending => "pending",
                    OrderStatus.Prepare => "preparing",
                    OrderStatus.Delivery => "delivery",
                    OrderStatus.Ready => "ready",
                    OrderStatus.Canceled => "canceled"
                };
                
                response.Message = $"Заказ {orderId} получил статус {statusRu}";

                var appUserCustomer = await _userRepository.GetUserById(userId);
                if (appUserCustomer?.IdentityHash is not null)
                {
                    var tgUser = await _telegramUserRepository.GetTelegramUserByHash(appUserCustomer.IdentityHash);

                    if (tgUser is not null)
                    {
                        var customerStatus = tgUser.Lang is LanguageTypes.RU ? statusRu : statusEn;
                        var customerTextEn = $"Your order({orderId}) has updated status to {customerStatus}";
                        var customerTextRu = $"Ваш заказ({orderId}) изменил статус: {customerStatus}";
                
                        var customerText = tgUser.Lang is LanguageTypes.RU ? customerTextRu : customerTextEn;
                
                        var customerNotification = new MessengerDataSendObject()
                        {
                            Message = customerText,
                            AppUser = appUserCustomer
                        };
                
                        AddMessageToInvoke(MessengerSourceType.Telegram, MessengerBotType.Customer, customerNotification);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }
        }
        
        return response;
    }
    
}