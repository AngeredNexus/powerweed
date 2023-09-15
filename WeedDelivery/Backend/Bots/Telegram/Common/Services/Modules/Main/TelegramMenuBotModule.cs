using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Models.Telegram;
using WeedDelivery.Backend.Models.Telegram.Menu;
using WeedDelivery.Backend.Models.Telegram.Menu.Common;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Services.Modules.Main;

public class TelegramMenuBotModule : TelegramBaseBotModule
{
    private readonly ILogger _logger;
    private readonly ITelegramUserRepository _telegramUserRepository;
    private readonly IUserRepository _userRepository;

    public override TelegramBotType BotType => TelegramBotType.MainBot;

    public TelegramMenuBotModule(ILogger<TelegramMenuBotModule> logger, ITelegramUserRepository telegramUserRepository, IUserRepository userRepository)
        : base(logger, telegramUserRepository, userRepository)
    {
        _logger = logger;
        _telegramUserRepository = telegramUserRepository;
        _userRepository = userRepository;
    }

    public override async Task HandleUpdateAsync(TelegramHandleRequestForm form)
    {
        _logger.LogDebug(" MenuBot received a '{MessageText}' message;\n" +
                         "In chat {ChatId};\n" +
                         "Username: {Usrnm}\n",
            form.TelegramMessage.Text, form.ChatId, form.TelegramMessage.Chat.Username);

        await Body(form);
    }

    private async Task Body(TelegramHandleRequestForm form)
    {
        if (form.User.Lang is LanguageTypes.Unknown)
        {
            await ConstructUserLangMenu(form);
            return;
        }

        if (!form.User.HasAcceptedLawPolicy)
        {
            await ConstructUserAgeMenu(form);
            return;
        }

        await ConstructMainMenu(form);
    }
    
    #region Menu's

    public override async Task HandleMenuCallback(TelegramHandleRequestForm form)
    {
        var data = form.TelegramUpdate.CallbackQuery!.Data ??
                   throw new ArgumentException("Menu callback has invalid data!");
        try
        {
            var response =
                JsonConvert.DeserializeObject<TelegramBaseResponse<JToken>>(data);

            switch (response.MenuType)
            {
                case TelegramBotMenuType.AgeMenu:
                    var menuAgeResponse =
                        JsonConvert.DeserializeObject<TelegramBotAgeMenuResponse>(response.Result.ToString());
                    await HandleAgeMenu(form, menuAgeResponse);
                    break;

                case TelegramBotMenuType.LanguageMenu:
                    var menuLangResponse =
                        JsonConvert.DeserializeObject<TelegramBotLanguageMenuReponse>(response.Result.ToString());
                    await HandleLangMenu(form, menuLangResponse);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Message: {Msg}\n" +
                             "Stack: {Stck}", ex.Message, ex.StackTrace);
        }
    }

    private async Task ConstructUserAgeMenu(TelegramHandleRequestForm form)
    {
        var title = form.User.Lang == LanguageTypes.RU ? "Вам есть 20+ лет?" : "Are you have 20+ years?";
        var positiveAnswer = form.User.Lang == LanguageTypes.RU ? "Да" : "Yes";
        var negativeAnswer = form.User.Lang == LanguageTypes.RU ? "Нет" : "No";


        TelegramBaseResponse<TelegramBotAgeMenuResponse> ageMenuPositiveResponse = new()
        {
            MenuType = TelegramBotMenuType.AgeMenu, Result = new TelegramBotAgeMenuResponse() { HasLegalAge = true }
        };

        TelegramBaseResponse<TelegramBotAgeMenuResponse> ageMenuNegativeResponse = new()
            { MenuType = TelegramBotMenuType.AgeMenu };


        var menuAgeItems = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: positiveAnswer,
                    callbackData: JsonConvert.SerializeObject(ageMenuPositiveResponse)),
                InlineKeyboardButton.WithCallbackData(text: negativeAnswer,
                    callbackData: JsonConvert.SerializeObject(ageMenuNegativeResponse))
            }
        });

        var msg = await BotClient.SendTextMessageAsync(form.ChatId, title, replyMarkup: menuAgeItems,
            cancellationToken: form.Token);
    }

    private async Task ConstructUserLangMenu(TelegramHandleRequestForm form)
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

        var msg = await BotClient.SendTextMessageAsync(form.ChatId, "Choose your language",
            replyMarkup: menuLanguageItems,
            cancellationToken: form.Token);
    }

    private async Task HandleAgeMenu(TelegramHandleRequestForm form, TelegramBotAgeMenuResponse response)
    {
        form.User.HasAcceptedLawPolicy = response.HasLegalAge;
        await _telegramUserRepository.UpdateUser(form.User, TelegramBotType.MainBot);
        await Body(form);
    }

    private async Task HandleLangMenu(TelegramHandleRequestForm form, TelegramBotLanguageMenuReponse response)
    {
        form.User.Lang = response.Language;
        await _telegramUserRepository.UpdateUser(form.User, TelegramBotType.MainBot);
        await Body(form);
    }

    private async Task ConstructMainMenu(TelegramHandleRequestForm form)
    {
        
        var sysUser = await _userRepository.GetUserByIdentity(IdentitySource.Telegram, $"{form.User.UserId}");
        var hash = sysUser?.IdentityHash ?? "";
        
        var locale = form.User.Lang == LanguageTypes.RU ? "ru/" : "";
        var appButtonText = form.User.Lang == LanguageTypes.RU ? "Магазин" : "Store";
        var groupButtonText = form.User.Lang == LanguageTypes.RU ? "Сообщество" : "Community";

        var notificationsBotText = form.User.Lang == LanguageTypes.RU ? "Заказы" : "Orders";
        var notificationsBotLink = "https://t.me/si_main_en_bot";
        
        var groupButtonLink = form.User.Lang == LanguageTypes.RU ? "https://t.me/+lK8pXxrTf041ZTUy" : "https://t.me/+enCFGiCL6pU2YjA6";
        
        _logger.LogInformation("Authorized {usr} for identity [{isrc} : {idnt}]", sysUser?.Name, sysUser?.Source.ToString(), hash);
        
        var menuItems = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithWebApp(appButtonText, new WebAppInfo()
                {
                    Url = $"https://testisland.store/?tgsh={hash}"
                }),
                InlineKeyboardButton.WithUrl(groupButtonText, groupButtonLink),
                InlineKeyboardButton.WithUrl(notificationsBotText, notificationsBotLink),
            }
        });
        

        // for (var id = 0; id < 66; id++)
        // {
        //
        //     try
        //     {
        //         await BotClient.DeleteMessageAsync(form.ChatId, id, form.Token);
        //     }
        //     catch (ApiRequestException e)
        //     {
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogError(e.Message);
        //     }
        //     
        // } 
        
        var msg = await BotClient.SendTextMessageAsync(form.ChatId, "Smoke Island Store Menu",
            replyMarkup: menuItems,
            cancellationToken: form.Token);
        
    }

    private async Task HandleMainMenu(TelegramHandleRequestForm form, TelegramBotAgeMenuResponse response)
    {
        
    }

#endregion
}