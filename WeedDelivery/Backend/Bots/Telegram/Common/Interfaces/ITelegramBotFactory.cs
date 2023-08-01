using WeedDatabase.Domain.Telegram.Types;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Interfaces;

public interface ITelegramBotFactory
{
    ITelegramBotModule GetModule(TelegramBotType botType);
}