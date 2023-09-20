using WeedDatabase.Domain.Telegram.Types;
using WeedDelivery.Backend.Models.Configuration.Bots;
using WeedDelivery.Backend.Systems.Messangers.Interfaces;
using WeedDelivery.Backend.Systems.Messangers.Models;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;

namespace WeedDelivery.Backend.Systems.Messangers.Services;

public class MessengerBotFactory : IMessengerBotFactory
{
    private readonly Dictionary<MessengerSourceType, List<IMessengerBotApiService>> _modulesMap;
    private readonly AppMessengerConfiguration _configuration;

    private readonly CancellationTokenSource _cts = new();
    
    public MessengerBotFactory(IEnumerable<IMessengerBotApiService> modules, Microsoft.Extensions.Options.IOptions<AppMessengerConfiguration> configuration)
    {
        _configuration = configuration.Value;
        _modulesMap = modules.GroupBy(x => x.MessengerSource).ToDictionary(x => x.Key, y => y.ToList());
        
        FireBotServers();
    }

    private void FireBotServers()
    {
        var tgModuleConfig = _configuration.Telegram.ToDictionary(x => x.BotType, y => y.Token);
        
        if(_modulesMap.TryGetValue(MessengerSourceType.Telegram, out var tgModules))
        {
            var listeners = tgModules.Select(x =>
            {
                x.Configure(new MessengerSetupObject()
                {
                    Token = tgModuleConfig[x.BotType],
                    CancellationTokenSource = _cts
                });

                Task.Run(() => RunMessageInvoker(x), _cts.Token);
                
                return x.StartAsync(_cts.Token);
            }).ToList();
        }
    }
    
    public IMessengerBotApiService GetSpecificBotInstance(MessengerSourceType source, MessengerBotType botType)
    {
        if (_modulesMap.TryGetValue(source, out var modules))
        {
            var module = modules.Single(x => x.BotType == botType);
            return module;
        }

        throw new ArgumentException($"Requested {botType.ToString()} bot not exists!");
    }

    private async Task RunMessageInvoker(IMessengerBotApiService messageSource)
    {
        foreach (var invokationMessage in await messageSource.AwaitMessages())
        {
            var invokableMessengerBot = GetSpecificBotInstance(invokationMessage.Messenger, invokationMessage.Type);
            await invokableMessengerBot.SendMessage(invokationMessage.MessageToSend);
        }
    }
}