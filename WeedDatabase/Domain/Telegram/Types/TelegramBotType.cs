namespace WeedDatabase.Domain.Telegram.Types;

public enum TelegramBotType
{
    Unknown = 0,
    
    MainBot = 1,

    OrderNotificationOperatorBot = 100,
    
    OrderNotificationCustomerBot = 101
}