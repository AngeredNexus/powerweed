using Database.Domain;
using Database.Repositories;
using WeedDelivery.Backend.Ecosystem.Auth;
using WeedDelivery.Backend.Ecosystem.Notifications;
using WeedDelivery.Backend.Ecosystem.Notifications.Messengers;
using WeedDelivery.Backend.Ecosystem.Users;

namespace WeedDelivery.Backend.Ecosystem.MessengerListeners.Telegram;

public class TelegramManagerBotService(ILogger<TelegramBotServiceBase> logger, IAuthService authService, IMessengerDataRepository messengerDataRepository, IUserCommonService userCommonService, IUserIdentityRepository userRepository, IMessengerDataRepository messengerData) 
    : TelegramBotServiceBase(logger, authService, messengerDataRepository, userCommonService, userRepository, "6631359689:AAGZafY7IGbke8dCI7DBSBaDb5MkXAd70A8")
        , IMessengerBotService
{
    private readonly ILogger _logger = logger;
    private readonly IUserIdentityRepository _userRepository = userRepository;
    private readonly IMessengerDataRepository _messengerDataRepository = messengerDataRepository;
    private readonly IAuthService _authService = authService;
    public virtual NotificationRole Role => NotificationRole.Manager;
    public virtual ContactType MessengerType => ContactType.TelegramManager;
    
    public async Task<object> SendMessageAsync(Guid contactDataId, string message)
    {
        var tgData = await messengerData.GetContactByIdAsync(contactDataId);

        if (tgData == null) return -1;
        
        await base.SendMessageAsync(tgData.ChatId, message);
        return 0;
    }

    protected override async Task HandleCode(string code, string chatId, string userName)
    {
        var currentContact = await _messengerDataRepository.GetContactByDataAsync(chatId, userName);
        if(currentContact == null)
        {
            await base.SendMessageAsync(chatId, "That user cannot be authorized! Attempt is logged for administrator.");
            return;
        }

        var currentAuthContact = await _messengerDataRepository.GetAuthByDataIdAsync(currentContact.Id);
        
        var user = await _userRepository.FindByContactIdAsync(currentAuthContact!.Id);

        if (user is null || !user.Roles.Contains("mngr"))
        {
            await base.SendMessageAsync(chatId, "That user cannot be authorized! Attempt is logged for administrator.");
            return;
        }
        
        var isHandleSucceded = await _authService.HandleCode(new ContactAuthData()
        {
            Code = code,
            AuthContact = currentAuthContact!
        });
        
        
        if(!isHandleSucceded)
        {
            _logger.LogDebug("Telegram user({UsrId}) sent invalid code({Code})!", chatId, code);
            await base.SendMessageAsync(chatId, "Authorization code timeout! Try login again with new code!");
        }
        
    }
}