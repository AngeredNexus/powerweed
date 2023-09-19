namespace WeedDelivery.Backend.Models.Configuration.Bots;

public class AppMessengerConfiguration
{
    public List<TelegramBotConfiguration> Telegram { get; set; } = new();
}