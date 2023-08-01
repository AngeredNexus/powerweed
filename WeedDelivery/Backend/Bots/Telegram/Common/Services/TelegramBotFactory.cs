using WeedDatabase.Domain.Telegram.Types;
using WeedDelivery.Backend.Bots.Telegram.Common.Interfaces;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Services;

public class TelegramBotFactory : ITelegramBotFactory
{
    private readonly Dictionary<TelegramBotType, ITelegramBotModule> _modulesMap;

    public TelegramBotFactory(IEnumerable<ITelegramBotModule> modules)
    {
        _modulesMap = modules.ToDictionary(x => x.BotType, y => y);
    }

    public ITelegramBotModule GetModule(TelegramBotType botType)
    {
        if(_modulesMap.TryGetValue(botType, out var module))
            return module;

        throw new ArgumentException($"Requested {botType.ToString()} bot not exists!");
    }
}