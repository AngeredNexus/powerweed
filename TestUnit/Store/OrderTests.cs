using Autofac;
using Database.Domain;
using Database.Repositories;
using Moq;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Ecosystem.Discount;
using WeedDelivery.Backend.Ecosystem.Store;
using WeedDelivery.Backend.Models.Api.Request;

namespace TestUnit.Store;

public class OrderTests
{
    
    #region Data

    public static readonly IEnumerable<Product> Products =
    [
        new Product()
        {
            Id = GuidExtensions.Sequential(),
            Name = "Super Silver Haze",
            Price = 300
        },

        new Product()
        {
            Id = GuidExtensions.Sequential(),
            Name = "V6 Haze",
            Price = 300
        },

        new Product()
        {
            Id = GuidExtensions.Sequential(),
            Name = "DevilDriver",
            Price = 400
        },

        new Product()
        {
            Id = GuidExtensions.Sequential(),
            Name = "Banana Daddy",
            Price = 500
        },

        new Product()
        {
            Id = GuidExtensions.Sequential(),
            Name = "Candy Man",
            Price = 400
        },

        new Product()
        {
            Id = GuidExtensions.Sequential(),
            Name = "Rose Gold",
            Price = 400
        }
    ];


    private static readonly List<ProductOrder> OrderNoDiscount =
    [
        new ProductOrder()
        {
            Id = GuidExtensions.Sequential(),
            Amount = 1,
            ProductId = Products.ElementAt(0).Id
        },

        new ProductOrder()
        {
            Id = GuidExtensions.Sequential(),
            Amount = 1,
            ProductId = Products.ElementAt(2).Id
        },

        new ProductOrder()
        {
            Id = GuidExtensions.Sequential(),
            Amount = 1,
            ProductId = Products.ElementAt(4).Id
        },
    ];
    
    private static readonly List<ProductOrder> OrderDiscount =
    [
        new ProductOrder()
        {
            Id = GuidExtensions.Sequential(),
            Amount = 3,
            ProductId = Products.ElementAt(0).Id
        },

        new ProductOrder()
        {
            Id = GuidExtensions.Sequential(),
            Amount = 3,
            ProductId = Products.ElementAt(2).Id
        },

        new ProductOrder()
        {
            Id = GuidExtensions.Sequential(),
            Amount = 3,
            ProductId = Products.ElementAt(4).Id
        },
    ];
    
    #endregion
    
    private readonly IContainer _container;

    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly IProductViewService _productViewService;
    private readonly IOrderService _orderService;
    private readonly IDiscountService _discountService;

    public OrderTests()
    {
        var iocContainerBuilder = new ContainerBuilder();

        // register types
        _mockProductRepository = new();
        _mockOrderRepository = new();

        iocContainerBuilder.RegisterInstance(_mockProductRepository.Object).As<IProductRepository>();
        iocContainerBuilder.RegisterInstance(_mockOrderRepository.Object).As<IOrderRepository>();
        iocContainerBuilder.RegisterType<ProductViewService>().As<IProductViewService>();
        iocContainerBuilder.RegisterType<ProductOrderService>().As<IOrderService>();
        iocContainerBuilder.RegisterType<BaseDiscountService>().As<IDiscountService>();

        _container = iocContainerBuilder.Build();
        _productViewService = _container.Resolve<IProductViewService>();
        _discountService = _container.Resolve<IDiscountService>();
        _orderService = _container.Resolve<IOrderService>();
    }

    
    [Fact]
    public async Task OrderNoDiscountOk()
    {
        _mockProductRepository.Setup(x => x.GetFilterId(It.IsAny<List<Guid>>())).ReturnsAsync(Products);
        _mockOrderRepository.Setup(x => x.WriteUserOrder(It.IsAny<UserOrder>(), It.IsAny<List<ProductOrder>>())).ReturnsAsync(true);
        
        var discounted = await _discountService.SetupDiscount(OrderNoDiscount);
        var discountedPrice = discounted.Sum(x => x.Updated * x.Order.Amount);
        Assert.Equal(1100, discountedPrice);
    }

    [Fact]
    public async Task OrderDiscountOk()
    {
        _mockProductRepository.Setup(x => x.GetFilterId(It.IsAny<List<Guid>>())).ReturnsAsync(Products);
        _mockOrderRepository.Setup(x => x.WriteUserOrder(It.IsAny<UserOrder>(), It.IsAny<List<ProductOrder>>())).ReturnsAsync(true);
        
        var discounted = await _discountService.SetupDiscount(OrderDiscount);
        var discountedPrice = discounted.Sum(x => x.Updated * x.Order.Amount);
        Assert.Equal(2640, discountedPrice);
    }
    
    [Fact]
    public async Task OrderPlaceOk()
    {
        _mockProductRepository.Setup(x => x.GetFilterId(It.IsAny<List<Guid>>())).ReturnsAsync(Products);
        _mockOrderRepository.Setup(x => x.WriteUserOrder(It.IsAny<UserOrder>(), It.IsAny<List<ProductOrder>>())).ReturnsAsync(true);
        
        var order = await _orderService.TryCreateOrder(new OrderRequestApi()
        {
            RelatedUser = new User(){ Id = GuidExtensions.Sequential(), Roles = ["cstmr"], ContactId = GuidExtensions.Sequential() },
            Address = "https://google.com/maps/pointer_data",
            Items = OrderNoDiscount
        });
        
        Assert.NotNull(order);
        Assert.Equal(1100, order.ProductPrice);
        Assert.Equal(150, order.DeliveryPrice);
        Assert.Equal(order.ProductPrice, order.DiscountedProdcutPrice);
    }
    
    [Fact]
    public async Task OrderWithDiscountPlaceOk()
    {
        _mockProductRepository.Setup(x => x.GetFilterId(It.IsAny<List<Guid>>())).ReturnsAsync(Products);
        _mockOrderRepository.Setup(x => x.WriteUserOrder(It.IsAny<UserOrder>(), It.IsAny<List<ProductOrder>>())).ReturnsAsync(true);
        
        var order = await _orderService.TryCreateOrder(new OrderRequestApi()
        {
            RelatedUser = new User(){ Id = GuidExtensions.Sequential(), Roles = ["cstmr"], ContactId = GuidExtensions.Sequential() },
            Address = "https://google.com/maps/pointer_data",
            Items = OrderDiscount
        });
        
        Assert.NotNull(order);
        Assert.Equal(3300, order.ProductPrice);
        Assert.Equal(150, order.DeliveryPrice);
        Assert.Equal(2640, order.DiscountedProdcutPrice);
        Assert.NotEqual(order.ProductPrice, order.DiscountedProdcutPrice);
    }
    
}