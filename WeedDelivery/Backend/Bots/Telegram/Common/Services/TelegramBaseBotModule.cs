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
    
    protected TelegramBotClient? _botClient;
    private string? _botToken;
    private CancellationToken _token;

    
    public abstract TelegramBotType BotType { get; }
    
    protected TelegramBaseBotModule(ILogger<TelegramBaseBotModule> logger, ITelegramUserRepository telegramUserRepository)
    {
        _logger = logger;
        _telegramUserRepository = telegramUserRepository;
    }

    public void StartBot(string? botToken, CancellationToken token, ReceiverOptions? options = null)
    {

        if (botToken is null)
        {
            _logger.LogError("Null bot token!");
            return;
        }
        
        _botToken = botToken;
        _token = token;
        
        options ??= new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _botClient = new TelegramBotClient(_botToken);
        _logger.LogInformation("Bot started with token: {Tkn}", _botToken);
        
        _botClient.StartReceiving(
            updateHandler: BaseHandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: options,
            cancellationToken: _token
        );
        
        _logger.LogInformation("Bot listening");
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
        StartBot(_botToken, _token);
    }

    public abstract Task HandleUpdateAsync(TelegramHandleRequestForm form);
    public abstract Task HandleMenuCallback(TelegramHandleRequestForm form);
    
    public async Task SendMessageAsync(string userId, string message)
    {
        throw new NotImplementedException();
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