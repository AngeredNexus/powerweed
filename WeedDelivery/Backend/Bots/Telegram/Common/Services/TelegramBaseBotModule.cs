using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeedDatabase.Domain.Telegram;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Bots.Telegram.Common.Interfaces;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Models.Telegram;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Services;

public abstract class TelegramBaseBotModule : ITelegramBotModule
{
    private readonly ILogger _logger;
    private readonly ITelegramUserRepository _telegramUserRepository;
    
    protected TelegramBotClient? BotClient;
    private string? _botToken;
    private CancellationToken _token;
    

    public abstract TelegramBotType BotType { get; }
    
    protected TelegramBaseBotModule(ILogger<TelegramBaseBotModule> logger, ITelegramUserRepository telegramUserRepository)
    {
        _logger = logger;
        _telegramUserRepository = telegramUserRepository;
    }

    public Task Listen(string? botToken, CancellationToken token, ReceiverOptions? options = null)
    {

        if (botToken is null)
        {
            _logger.LogError("Null bot token!");
            return Task.FromException<ArgumentException>(new ArgumentException("Empty bot token for {BtTp} !", BotType.ToString()));
        }
        
        _botToken = botToken;
        _token = token;
        
        options ??= new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        BotClient = new TelegramBotClient(_botToken);
        _logger.LogInformation("Bot started with token: {Tkn}", _botToken);
        
        BotClient.StartReceiving(
            updateHandler: BaseHandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: options,
            cancellationToken: _token
        );
        
        _logger.LogInformation("Bot listening");

        return Task.CompletedTask;
    }
    
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"(T-API):\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => $"(OTH):\n{exception.Message}\n" +
                 $"Stacktrace: {exception.StackTrace}"
        };

        _logger.LogError("Error during polling telegram {ErrMsg}", errorMessage);
        _logger.LogInformation("Bot {Tkn} restarting...", _token);
        await Listen(_botToken, _token);
    }

    public abstract Task HandleUpdateAsync(TelegramHandleRequestForm form);
    public abstract Task HandleMenuCallback(TelegramHandleRequestForm form);
    
    public virtual async Task SendMessageAsync(string userId, string message)
    {
        if(BotClient is null)
            return;
        
        var msg = await BotClient.SendTextMessageAsync(userId, message, cancellationToken: _token);
    }

    public virtual async Task SendMessageAsync<T>(string userId, string message, T data) where T : class
    {
        await SendMessageAsync(userId, message);
    }
    
    private async Task BaseHandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var requestForm = new TelegramHandleRequestForm(
            null,
            botClient,
            update,
            cancellationToken);
        
        if (await BaseHandleMenuCallback(botClient, update, cancellationToken))
        {
            await SetupUser(requestForm);
            await HandleMenuCallback(requestForm);
            return;
        }
        
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        
        await SetupUser(requestForm);
        await HandleUpdateAsync(requestForm);
    }

    private async Task<bool> BaseHandleMenuCallback(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if(update.CallbackQuery is not {} callbackQuery)
            return false;
        
        if(update.CallbackQuery.Data is not {} data)
            return false;

        return true;
    }

    private async Task SetupUser(TelegramHandleRequestForm form)
    {
        var user = new TelegramBotUser()
        {
            Id = Guid.Empty.Sequential(),
            UserId = form.TelegramMessage.Chat.Id,
            BotType = BotType,
            IsActive = true
        };
        
        user = await _telegramUserRepository.InsertOrGetExisted(user, BotType);

        if (form.TelegramMessage.Text!.Contains("/start"))
        {
            user.IsActive = true;
            await _telegramUserRepository.UpdateUser(user, BotType);
        }
        
        if (form.TelegramMessage.Text.Contains("/stop"))
        {
            user.IsActive = false;
            await _telegramUserRepository.UpdateUser(user, BotType);
        }

        form.User = user;
    }
    
}