using Database.Domain;

namespace WeedDelivery.Backend.Ecosystem.Discount;

public interface IDiscountService
{
    Task<List<DiscountOrder>> SetupDiscount(List<ProductOrder> orders);
}