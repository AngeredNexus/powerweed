using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
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

    public override TelegramBotType BotType => TelegramBotType.MainBot;

    public TelegramMenuBotModule(ILogger<TelegramMenuBotModule> logger, ITelegramUserRepository telegramUserRepository)
        : base(logger, telegramUserRepository)
    {
        _logger = logger;
        _telegramUserRepository = telegramUserRepository;
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
        var locale = form.User.Lang == LanguageTypes.RU ? "ru/" : "";
        var appButtonText = form.User.Lang == LanguageTypes.RU ? "Магазин" : "Store";
        var groupButtonText = form.User.Lang == LanguageTypes.RU ? "Сообщество" : "Community";

        var groupButtonLink = form.User.Lang == LanguageTypes.RU ? "https://t.me/+qjsVsv8-3d4yMWMy" : "https://t.me/+_DjxP8VvpWcwODJi";
        
        var menuItems = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithWebApp(appButtonText, new WebAppInfo()
                {
                    Url = "https://smokeisland.store/"
                }),
                InlineKeyboardButton.WithUrl(groupButtonText, groupButtonLink),
            }
        });

        var msg = await BotClient.SendTextMessageAsync(form.ChatId, "<- Smoke Island ->",
            replyMarkup: menuItems,
            cancellationToken: form.Token);
        
    }

    private async Task HandleMainMenu(TelegramHandleRequestForm form, TelegramBotAgeMenuResponse response)
    {
        
    }

#endregion
}