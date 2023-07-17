using Telegram.Bot;
using Telegram.Bot.Types;
using WeedDatabase.Domain.Telegram;

namespace WeedDelivery.Backend.Models.Telegram;

public class TelegramHandleRequestForm
{
    public TelegramHandleRequestForm(TelegramBotUser user, ITelegramBotClient botClient, Update telegramUpdate, CancellationToken token)
    {
        User = user;
        BotClient = botClient;
        TelegramUpdate = telegramUpdate;
        Token = token;
    }

    public TelegramBotUser User { get; set; }
    
    public ITelegramBotClient BotClient { get; set; }
    
    public Update TelegramUpdate { get; set; }

    public CancellationToken Token { get; set; }

    public Message TelegramMessage => TelegramUpdate.Message ?? TelegramUpdate.CallbackQuery.Message ?? throw new ArgumentNullException("TelegramUpdate have null message!");

    public long ChatId => TelegramMessage.Chat.Id;
}