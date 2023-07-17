using Autofac;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.App.Common.Services;
using WeedDelivery.Backend.App.Market.Admin.Repos;
using WeedDelivery.Backend.App.Market.Customer.Repos;
using WeedDelivery.Backend.App.Ordering.Repos;
using WeedDelivery.Backend.Bots.Telegram.Common;

namespace WeedDelivery.Backend.AppInit.Deps;

public class AppDependencyDatabaseContextModule : Module
{
   
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MarketAdminItemsRepository>().As<IMarketAdminItemsRepository>();
        builder.RegisterType<MarketCustomerItemsRepository>().As<IMarketCustomerItemsRepository>();
        builder.RegisterType<MarketAdminOrderRepository>().As<IMarketAdminOrderRepository>();
        builder.RegisterType<MarketCustomerOrdersRepository>().As<IMarketCustomerOrdersRepository>();
        
        builder.RegisterType<TelegramUserRepository>().As<ITelegramUserRepository>();
        
        builder.RegisterType<WeedContextAcceptor>().As<IWeedContextAcceptor>();
        builder.RegisterType<TelegramContextAcceptor>().As<ITelegramContextAcceptor>();
    }
}