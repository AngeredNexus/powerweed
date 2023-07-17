namespace WeedDatabase.Domain.Telegram.Types;

public enum TelegramBotType
{
    Unknown = 0,
    
    MainBot = 1,
    
    OrderNotificationCustomerBot = 100,
    OrderNotificationOperatorBot = 101
}