using System.Net;
using Database.Domain;
using Database.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Ecosystem.Auth;
using WeedDelivery.Backend.Ecosystem.Users;

namespace WeedDelivery.Backend.Ecosystem.MessengerListeners.Telegram;

public abstract class TelegramBotServiceBase(
    ILogger<TelegramBotServiceBase> logger,
    IAuthService authService,
    IMessengerDataRepository messengerDataRepository,
    IUserCommonService userCommonService,
    IUserIdentityRepository userRepository, string botToken)
    : BackgroundService
{
    private readonly ILogger _logger = logger;
    
    protected virtual TelegramBotClient? BotClient { get; set; }
    private CancellationToken _token;


    Task ConfigureClient()
    {
        BotClient = new TelegramBotClient(botToken);
        
        var options = new ReceiverOptions()
        {
            AllowedUpdates = []
        };
        
        BotClient.StartReceiving(
            updateHandler: BaseHandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: options,
            cancellationToken: CancellationToken.None
        );
        
        return Task.CompletedTask;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _token = stoppingToken;

        await ConfigureClient();
        _logger.LogInformation("Telegram bot(token: {Tkn}) started.", botToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        //SetupConfiguration.CancellationTokenSource.Cancel(true);
    }

    protected abstract Task HandleCode(string code, string chatId, string userName);

    public virtual async Task BaseHandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is null && update.CallbackQuery is null)
            return;

        var message = update.Message ?? update.CallbackQuery?.Message;

        if (update.CallbackQuery is not null)
            if (message is not null)
                message.Text = update.CallbackQuery.Data;
            else
            {
                _logger.LogWarning("Empty message from TG callback query with chatId: {Id}", update.CallbackQuery.From.Id);
                return;
            }
        
        if(message is not null && !string.IsNullOrWhiteSpace(message.Chat.Username))
            HandleCode(message.Text ?? string.Empty, message.Chat.Id.ToString(), message.Chat.Username);
    }

    protected virtual async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = "Telegram bot(token: {Tkn}) encountered an error.";


        var apiRequestException = exception as ApiRequestException;
        if (apiRequestException is { ErrorCode: (int)HttpStatusCode.Conflict }) return;
        
        apiRequestException ??= new ApiRequestException(errorMessage);
        _logger.LogError("Error during polling telegram {ErrMsg}", apiRequestException.Message);

        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        await ConfigureClient();
    }

    protected async Task<object> SendMessageAsync(string chatId, string message)
    {
        if (BotClient is null)
        {
            _logger.LogWarning("Bot client is not configured!");
            await ConfigureClient();
            return SendMessageAsync(chatId, message);
        }
        
        await BotClient.SendTextMessageAsync(chatId, message,
                replyMarkup: null, cancellationToken: _token);
        
        return 0;
    }
}