using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Models.Telegram;
using WeedDelivery.Backend.Models.Telegram.Menu;
using WeedDelivery.Backend.Models.Telegram.Menu.Common;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Services.Modules.Notification;

public class TelegramAdminGeneralBotModule : TelegramBaseBotModule
{
    private readonly ILogger _logger;
    private readonly ITelegramUserRepository _telegramUserRepository;

    public override TelegramBotType BotType => TelegramBotType.OrderNotificationOperatorBot;

    public TelegramAdminGeneralBotModule(ILogger<TelegramAdminGeneralBotModule> logger, ITelegramUserRepository telegramUserRepository)
        : base(logger, telegramUserRepository)
    {
        _logger = logger;
        _telegramUserRepository = telegramUserRepository;
    }

    
    
    public override async Task HandleUpdateAsync(TelegramHandleRequestForm form)
    {
        _logger.LogDebug("Operator bot received a '{MessageText}' message;\n" +
                         "In chat {ChatId};\n" +
                         "Username: {Usrnm}\n",
            form.TelegramMessage.Text, form.ChatId, form.TelegramMessage.Chat.Username);

        
        // place shield from unregistered id's after
        
        // await Body(form);
    }

    #region Menu's

    public override async Task HandleMenuCallback(TelegramHandleRequestForm form)
    {
        return;
        
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
    private async Task HandleAgeMenu(TelegramHandleRequestForm form, TelegramBotAgeMenuResponse response)
    {
        form.User.HasAcceptedLawPolicy = response.HasLegalAge;
        await _telegramUserRepository.UpdateUser(form.User, TelegramBotType.MainBot);
        // await Body(form);
    }
    

#endregion
}