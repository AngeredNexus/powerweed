using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Models.Telegram;
using WeedDelivery.Backend.Systems.Messangers.Interfaces;
using WeedDelivery.Backend.Systems.Messangers.Models;
using WeedDelivery.Backend.Systems.Messangers.Models.MessengerSendingMessageObject;
using StringExtensions = WeedDelivery.Backend.Common.Utils.StringExtensions;

namespace WeedDelivery.Backend.Systems.Messangers.Services.HostedMessengerWorker;

public abstract class TelegramBotApiService : MessengerBotApiBaseService, ITelegramMessengerBotClient
{
    private readonly ILogger _logger;
    private readonly ITelegramUserRepository _telegramUserRepository;
    private readonly IUserRepository _userRepository;

    protected TelegramBotApiService(ILogger logger, ITelegramUserRepository telegramUserRepository,
        IUserRepository userRepository)
    {
        _logger = logger;
        _telegramUserRepository = telegramUserRepository;
        _userRepository = userRepository;
    }


    protected TelegramBotClient? BotClient { get; set; }
    protected MessengerSetupObject SetupConfiguration { get; set; }

    public sealed override MessengerSourceType MessengerSource => MessengerSourceType.Telegram;

    public override void Configure(MessengerSetupObject setup)
    {
        SetupConfiguration = setup;

        if (string.IsNullOrWhiteSpace(SetupConfiguration.Token))
        {
            _logger.LogError("Null bot token!");
            throw new ArgumentException("Empty bot token for {BtTp} !", MessengerSource.ToString());
        }


        BotClient = new TelegramBotClient(SetupConfiguration.Token);
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var options = new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        if (BotClient is null)
            throw new NullReferenceException($"Bot client cannot be started: client is not configured! " +
                                             $"Config: {JsonConvert.SerializeObject(SetupConfiguration)}");

        BotClient.StartReceiving(
            updateHandler: BaseHandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: options,
            cancellationToken: SetupConfiguration.CancellationTokenSource.Token
        );

        _logger.LogInformation("Internal async server for BOT({BTTP}) of MESSENGER({MSGR}) started!", BotType,
            MessengerSource);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        SetupConfiguration.CancellationTokenSource.Cancel(true);
    }


    public virtual async Task BaseHandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is null && update.CallbackQuery is null)
            return;

        var message = update.Message ?? update.CallbackQuery?.Message;

        if (update.CallbackQuery is not null)
            if (message is not null)
                message.Text = update.CallbackQuery.Data;

        var hash = $"{SetupConfiguration.Token}.{message?.Chat.Id}".ComputeSha512();
        var requestForm = new TelegramHandleRequestForm(update, hash);

        var msgrUser = await _telegramUserRepository.GetTelegramUserByHash(hash);
        var appUser = await _userRepository.GetUserByIdentityHash(hash);

        if (appUser is null)
        {
            appUser = new SmokiUser()
            {
                Name = message?.Chat.Username,
                Role = SmokiUserRole.Customer,
                Source = MessengerSourceType.Telegram,
                SourceIdentificator = message?.Chat.Id.ToString() ?? "",
                IdentityHash = hash,
                Code = StringExtensions.GetRandomAlphanumericString(12)
            };

            await _userRepository.AddUser(appUser);
        }

        if (msgrUser is null)
        {
            msgrUser = new TelegramBotUser()
            {
                UserId = message?.Chat.Id ?? -1,
                MessengerSource = MessengerSourceType.Telegram,
                IsActive = true,
                Hash = hash,
                HasAcceptedLawPolicy = true,
                AppUserId = appUser.Id,
                Lang = LanguageTypes.EN
            };

            await _telegramUserRepository.InsertOrGetExisted(msgrUser);
        }

        requestForm.AppMessage = new MessengerDataUpdateObject()
        {
            Hash = hash,
            Message = message?.Text,
            AppUser = appUser
        };

        requestForm.User = msgrUser;
        var response = await HandleTelegramMessegeInput(requestForm);

        response.AppUser = appUser;
        await SendMessage(response);
    }

    public virtual async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"(T-API):\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => $"(OTH):\n{exception.Message}\n" +
                 $"Stacktrace: {exception.StackTrace}"
        };

        _logger.LogError("Error during polling telegram {ErrMsg}", errorMessage);
        _logger.LogInformation("Bot {Tkn} restarting after 5 sec...", SetupConfiguration.Token);

        await Task.Delay(5000, SetupConfiguration.CancellationTokenSource.Token);

        if (!SetupConfiguration.CancellationTokenSource.IsCancellationRequested)
            await StartAsync(SetupConfiguration.CancellationTokenSource.Token);
    }

    public override async Task SendMessage(MessengerDataSendObject message)
    {
        if (BotClient is null || message.AppUser is null || string.IsNullOrWhiteSpace(message.Message))
            return;

        var chatId = message.AppUser.SourceIdentificator;
        var chatMessage = message.Message;

        IReplyMarkup? replyMarkup = null;

        if (message.MessageObject is not null)
        {
            var messageObject = message.MessageObject.GetType().IsAssignableTo(typeof(TelegramSendingMessage))
                ? (TelegramSendingMessage)message.MessageObject
                : null;
            replyMarkup = messageObject?.Markup;
        }

        await BotClient.SendTextMessageAsync(chatId, chatMessage,
            replyMarkup: replyMarkup, cancellationToken: SetupConfiguration.CancellationTokenSource.Token);
    }

    public abstract Task<MessengerDataSendObject> HandleTelegramMessegeInput(TelegramHandleRequestForm form);
}