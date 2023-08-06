using WeedDatabase.Context.Interfaces;
using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
using WeedDatabase.Repositories;

namespace WeedDelivery.Backend.App.Market.Customer.Repos;

public class MarketCustomerOrdersRepository : IMarketCustomerOrdersRepository
{
    
    private readonly IWeedContextAcceptor _contextAcceptor;
    private readonly ILogger _logger;

    public MarketCustomerOrdersRepository(IWeedContextAcceptor contextAcceptor, ILogger<MarketCustomerOrdersRepository> logger)
    {
        _contextAcceptor = contextAcceptor;
        _logger = logger;
    }
    
    public async Task<bool> TryToPlace(Order order)
    {
        bool isSuccess;
        
        await using var dbCtx = _contextAcceptor.CreateContext();
        var now = DateTime.Now;
        
        try
        {
            order.Created = now;
            order.Modified = now;
            order.Items.ForEach(x =>
            {
                x.Created = now;
                x.Modified = now;
            });
            
            dbCtx.Orders.Add(order);
            dbCtx.OrderItems.AddRange(order.Items);
            
            await dbCtx.SaveChangesAsync();
            isSuccess = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка вставки заказа! Данные: {msg} \n Стэк: {stk} \n Inner: {imsg} InnerStk: {istk}", ex.Message, ex.StackTrace, ex.InnerException?.Message, ex.InnerException?.StackTrace);
            isSuccess = false;
        }
        
        return isSuccess;
    }
}