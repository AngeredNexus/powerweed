using Database.Domain;
using Database.Repositories;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Ecosystem.Auth;
using WeedDelivery.Backend.Ecosystem.Notifications;
using WeedDelivery.Backend.Ecosystem.Notifications.Messengers;
using WeedDelivery.Backend.Ecosystem.Users;

namespace WeedDelivery.Backend.Ecosystem.MessengerListeners.Telegram;

public class TelegramCustomerBotService(ILogger<TelegramBotServiceBase> logger, IAuthService authService, IMessengerDataRepository messengerDataRepository, IUserCommonService userCommonService, IUserIdentityRepository userRepository, IMessengerDataRepository messengerData) 
    :TelegramBotServiceBase(logger, authService, messengerDataRepository, userCommonService, userRepository, "6385943140:AAGA6iteFlngxwVy9UxjmuOtU4oyvW9pa1s")
        ,IMessengerBotService
{
    private readonly IMessengerDataRepository _messengerDataRepository = messengerDataRepository;
    private readonly IAuthService _authService = authService;
    private readonly ILogger _logger = logger;
    
    public virtual NotificationRole Role => NotificationRole.Customer;
    public virtual ContactType MessengerType => ContactType.Telegram;
    
    public async Task<object> SendMessageAsync(Guid contactDataId, string message)
    {
        var tgData = await messengerData.GetContactByIdAsync(contactDataId);
        
        if(tgData != null)
        {
            await base.SendMessageAsync(tgData.ChatId, message);
            return 0;
        }

        return -1;
    }

    protected override async Task HandleCode(string code, string chatId, string userName)
    {
        var currentContact = await _messengerDataRepository.GetContactByDataAsync(chatId, userName) 
                             ?? new TelegramContact()
                                    {
                                        Id = GuidExtensions.Sequential(),
                                        ChatId = chatId,
                                        Username = userName
                                    };

        var currentAuthContact = await _messengerDataRepository.GetAuthByDataIdAsync(currentContact.Id)
                                 ?? new AuthContact()
                                 {
                                     Id = GuidExtensions.Sequential(),
                                     Type = ContactType.Telegram,
                                     DataId = currentContact.Id,
                                 };
        
        currentContact.ContactId = currentAuthContact.Id;
        await _messengerDataRepository.TryInsertAuthContactWithDataAsync(currentAuthContact, currentContact);
        
        var isHandleSucceded = await _authService.HandleCode(new ContactAuthData()
        {
            Code = code,
            AuthContact = currentAuthContact
        });
        
        if(!isHandleSucceded)
        {
            _logger.LogDebug("Telegram user({UsrId}) sent invalid code({Code})!", chatId, code);
            await base.SendMessageAsync(chatId, "Authorization code timeout! Try login again with new code!");
        }
    }
}