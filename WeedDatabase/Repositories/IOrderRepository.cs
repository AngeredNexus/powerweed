using Database.Domain;

namespace Database.Repositories;

public interface IOrderRepository
{
    Task<bool> WriteUserOrder(UserOrder userOrder, List<ProductOrder> productOrders);
}