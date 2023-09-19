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

public class TelegramBotMenuModule : TelegramBotApiService
{

    // 1. Оставить выбор языка как InlineKeyboard +
    // 2. Реализовать набор команд через объект MenuKeyboardMarkup +-
    // 3. Собрать объект сообщения ++
    // 4. Profit ++-

    public override MessengerBotType BotType => MessengerBotType.Customer;
    
    private readonly ITelegramUserRepository _telegramUserRepository;
    private readonly IUserRepository _userRepository;
    
    public TelegramBotMenuModule(ILogger<TelegramBotMenuModule> logger, ITelegramUserRepository telegramUserRepository, IUserRepository userRepository) : base(logger, telegramUserRepository, userRepository)
    {
        _telegramUserRepository = telegramUserRepository;
        _userRepository = userRepository;
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
                MessengerCommand.Start or MessengerCommand.Stop => await HandleInitCommands(form, command, arguments),
                MessengerCommand.Auth => await HandleAuthCommand(form),
                MessengerCommand.Language => await HandleLanguageCommand(form, arguments),
                MessengerCommand.SetLanguage => await HandleSetLanguageCommand(form, arguments),
            
                _ => throw new ArgumentException("Unknown command")
            };
        
            return response;
        }

        return new MessengerDataSendObject();
    }


    private async Task<MessengerDataSendObject> HandleInitCommands(TelegramHandleRequestForm form, MessengerCommand command, IReadOnlyCollection<string> arguments)
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
            return await HandleLanguageCommand(form, arguments);
        }

        return response;
    }
    
    private async Task<MessengerDataSendObject> HandleLanguageCommand(TelegramHandleRequestForm form, IReadOnlyCollection<string> arguments)
    {
        TelegramBaseResponse<TelegramBotLanguageMenuReponse> languageMenuResponse = new()
            { MenuType = TelegramBotMenuType.LanguageMenu, Result = new TelegramBotLanguageMenuReponse() };

        var menuLanguageItems = new ReplyKeyboardMarkup(Enum.GetValues<LanguageTypes>()
            .Except(new[] { LanguageTypes.Unknown }).Select(x =>
            {
                languageMenuResponse.Result.Language = x;
                return new KeyboardButton(text: $"/setlang {x.ToString().ToLower()}");
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

    private async Task<MessengerDataSendObject> HandleSetLanguageCommand(TelegramHandleRequestForm form,
        IReadOnlyCollection<string> arguments)
    {
        if (form.User is null)
            return new MessengerDataSendObject();
        
        if (arguments.First() == "ru")
        {
            // set lng ru
            form.User.Lang = LanguageTypes.RU;
        }
        else if (arguments.First() == "en")
        {
            form.User.Lang = LanguageTypes.EN;
        }

        await _telegramUserRepository.UpdateUser(form.User);

        var message = form.User.Lang == LanguageTypes.RU ? "Используемый язык был сохранен!" : "Language settings saved!";

        return new MessengerDataSendObject()
        {
            Message = message,
            MessageObject = new TelegramSendingMessage()
            {
                Markup = new ReplyKeyboardRemove()
            }
        };
    }
    
}