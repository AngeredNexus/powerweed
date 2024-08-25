using Autofac;
using Database.Domain;
using Database.Repositories;
using Moq;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Ecosystem.Store;

namespace TestUnit.Store;

public class ProductViewTests
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

    #endregion

    private readonly IContainer _container;
    
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly IProductViewService _productViewService;
    
    public ProductViewTests()
    {
        var iocContainerBuilder = new ContainerBuilder();
        
        // register types
        _mockProductRepository = new();
        
        iocContainerBuilder.RegisterInstance(_mockProductRepository.Object).As<IProductRepository>();
        iocContainerBuilder.RegisterType<ProductViewService>().As<IProductViewService>();

        _container = iocContainerBuilder.Build();
        _productViewService = _container.Resolve<IProductViewService>();

    }

    [Fact]
    public async Task ProductListOk()
    {
        _mockProductRepository.Setup(x => x.GetAll()).ReturnsAsync(Products);
        
        // call service
        // check some part of results to input data to be sure service didnt spoil something

        var result = await _productViewService.GetProductsAsync();

        var test = Products.ToHashSet();
        test.ExceptWith(result);
        
        Assert.Empty(test);

    }
    
    void ProductListByIdOk()
    {
        _mockProductRepository.Setup(x => x.GetAll()).ReturnsAsync(Products);
        
        // call service
        // check same length and id sets
    }
}