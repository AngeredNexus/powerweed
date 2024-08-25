using Database.Domain;
using Database.Repositories;

namespace WeedDelivery.Backend.Ecosystem.Discount;

public class BaseDiscountService : IDiscountService
{

    private readonly IProductRepository _productRepository;

    public BaseDiscountService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<DiscountOrder>> SetupDiscount(List<ProductOrder> orders)
    {
        var discounted = new List<DiscountOrder>();

        var pdksids = orders.Select(x => x.ProductId).ToList();
        var pdks = (await _productRepository.GetFilterId(pdksids)).ToList();
        var products = pdks.ToDictionary(x => x.Id, x => x.Price);
        
        var totalAmount = orders.Sum(x => x.Amount);
        
        orders.ForEach(x =>
        {
            if (products.TryGetValue(x.ProductId, out var price))
            {
                discounted.Add(new DiscountOrder { Order = x, Original = price, Updated = totalAmount >= 5 ? price - (decimal)price / 100 * 20 : price });
            }
        });
        
        return discounted;
    }
}