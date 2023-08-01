namespace WeedDelivery.Backend.Models.Configuration.Bots;

public class AppTelegramConfiguration
{
    public List<TelegramBotConfiguration> Bots { get; set; } = new();
}