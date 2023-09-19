using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using WeedDatabase.Domain.Common;
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
    
    public TelegramBotOperatorModule(ILogger<TelegramBotOperatorModule> logger, ITelegramUserRepository telegramUserRepository, IUserRepository userRepository) : base(logger, telegramUserRepository, userRepository)
    {
        _telegramUserRepository = telegramUserRepository;
        _userRepository = userRepository;
    }
    
    public override async Task<MessengerDataSendObject> HandleTelegramMessegeInput(TelegramHandleRequestForm form)
    {
        return new MessengerDataSendObject();
    }


    private async Task<MessengerDataSendObject> HandleInitCommands(TelegramHandleRequestForm form, MessengerCommand command)
    {
        var response = new MessengerDataSendObject()
        {
            Message = string.Empty,
            MessageObject = null
        };

        if (form.User is null)
            return response;
        
        var tgUser = await _telegramUserRepository.GetTelegramUserByHash(form.User.Hash);

        if (tgUser is null)
            return response;
        
        if (command is MessengerCommand.Stop)
        {
            tgUser.IsActive = false;
        }
        else if (command is MessengerCommand.Start)
        {
            tgUser.IsActive = true;
            return await HandleLanguageCommand(form);
        }

        return response;
    }
    
    private async Task<MessengerDataSendObject> HandleLanguageCommand(TelegramHandleRequestForm form)
    {
        TelegramBaseResponse<TelegramBotLanguageMenuReponse> languageMenuResponse = new()
            { MenuType = TelegramBotMenuType.LanguageMenu, Result = new TelegramBotLanguageMenuReponse() };

        var menuLanguageItems = new InlineKeyboardMarkup(Enum.GetValues<LanguageTypes>()
            .Except(new[] { LanguageTypes.Unknown }).Select(x =>
            {
                languageMenuResponse.Result.Language = x;
                return InlineKeyboardButton.WithCallbackData(text: x.ToString(),
                    callbackData: JsonConvert.SerializeObject(languageMenuResponse));
            }));

        var message = new MessengerDataSendObject()
        {
            Message = "LANGUAGE | ЯЗЫК",
            MessageObject = new TelegramSendingMessage()
            {
                Markup = menuLanguageItems
            }
        };

        return message;
    }
    
    private async Task<MessengerDataSendObject> HandleAuthCommand(TelegramHandleRequestForm form)
    {

        if (form.User is null)
            return new MessengerDataSendObject();
        
        var label = form.User.Lang is LanguageTypes.EN ? "Your store access code is:" : "Ваш код доступа к магазину:";
        
        var message = new MessengerDataSendObject()
        {
            Message = $"{label} {form.AppMessage.AppUser.Code}",
        };

        return message;
    }
}