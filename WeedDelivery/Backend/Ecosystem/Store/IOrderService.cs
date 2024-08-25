using Database.Domain;
using WeedDelivery.Backend.Models.Api.Request;

namespace WeedDelivery.Backend.Ecosystem.Store;

public interface IOrderService
{
    Task<UserOrder> TryCreateOrder(OrderRequest order);
}