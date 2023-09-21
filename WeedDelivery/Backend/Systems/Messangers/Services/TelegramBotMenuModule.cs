using Telegram.Bot.Types.ReplyMarkups;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Models.Telegram;
using WeedDelivery.Backend.Systems.App.Common;
using WeedDelivery.Backend.Systems.Messangers.Models;
using WeedDelivery.Backend.Systems.Messangers.Models.MessengerSendingMessageObject;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;
using WeedDelivery.Backend.Systems.Messangers.Services.HostedMessengerWorker;

namespace WeedDelivery.Backend.Systems.Messangers.Services;

public class TelegramBotMenuModule : TelegramBotApiService
{
    
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
            
                _ => await HandleMenuCommand(form, arguments)
            };
        
            return response;
        }

        return await HandleMenuCommand(form, new List<string>());
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
        
        var menuCommand = await HandleMenuCommand(form, arguments);
        menuCommand.AppUser = form.AppMessage.AppUser;
        
        AddMessageToInvoke(MessengerSourceType.Telegram, MessengerBotType.Customer, menuCommand);
        
        var message = form.User.Lang == LanguageTypes.RU ? "Используемый язык был сохранен!" : "Language settings saved!";

        return new MessengerDataSendObject()
        {
            Message = message
        };
        
    }
    
    private async Task<MessengerDataSendObject> HandleLanguageCommand(TelegramHandleRequestForm form, IReadOnlyCollection<string> arguments)
    {
        var menuLanguageItems = new InlineKeyboardMarkup(new []
        {
            InlineKeyboardButton.WithCallbackData("Русский", "/setlang ru"),
            InlineKeyboardButton.WithCallbackData("English", "/setlang en"),
        });

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
    
    private async Task<MessengerDataSendObject> HandleMenuCommand(TelegramHandleRequestForm form, IReadOnlyCollection<string> arguments)
    {

        var menuText = form.User.Lang is LanguageTypes.RU ? "Меню" : "Menu";
        var communityText = form.User.Lang is LanguageTypes.RU ? "Сообщество" : "Community";
        var shopText = form.User.Lang is LanguageTypes.RU ? "Магазин" : "Shop";
        var languageText = form.User.Lang is LanguageTypes.RU ? "Язык" : "Language";
        var codeText = form.User.Lang is LanguageTypes.RU ? "Код доступа" : "Auth code";
        
        var communityLink = form.User.Lang is LanguageTypes.RU ? "https://t.me/smokeislandru" : "https://t.me/smokeislanden";
        
        var menuLanguageItems = new InlineKeyboardMarkup(new []
        {
            InlineKeyboardButton.WithUrl(shopText, "https://smokeisland.store"),
            InlineKeyboardButton.WithCallbackData(codeText, "/passwd"),
            InlineKeyboardButton.WithCallbackData(languageText, "/lang"),
            InlineKeyboardButton.WithUrl(communityText, communityLink),
        });

        var message = new MessengerDataSendObject()
        {
            Message = menuText,
            MessageObject = new TelegramSendingMessage()
            {
                Markup = menuLanguageItems
            }
        };

        return message;
    }
}