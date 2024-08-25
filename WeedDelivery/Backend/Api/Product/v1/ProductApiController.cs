using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Ecosystem.Store;
using WeedDelivery.Backend.Models.Api.Response;

namespace WeedDelivery.Backend.Api.Product.v1;

[ApiController]
[ApiVersion("1")]
[Microsoft.AspNetCore.Mvc.Route("api/{version}/product")]
public class ProductApiController : Controller
{
    
    private readonly IProductViewService _productService;
    private readonly IProductAdditionalInfoService _productAdditionalInfoService;

    public ProductApiController(IProductViewService productService, IProductAdditionalInfoService productAdditionalInfoService)
    {
        _productService = productService;
        _productAdditionalInfoService = productAdditionalInfoService;
    }

    [HttpGet]
    [Authorize(Roles = "cstmr")]
    [Microsoft.AspNetCore.Mvc.Route("all")]
    public async Task<ActionResult<ProductList>> All()
    {
        var products = await _productService.GetProductsAsync();

        // additional info
        var ids = products.Select(p => p.Id).ToList();
        var images = await _productAdditionalInfoService.GetProductImagesBatch(ids);
        var specifications = await _productAdditionalInfoService.GetProductSpecificationnBatch(ids);
        
        var result = new ProductList
        {
            Products = products.Select(p => new ApiProduct()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Images = images.TryGetValue(p.Id, out var pImages) ? pImages.Select(x => x.Value).ToList() : new(),
                Specification = specifications.TryGetValue(p.Id, out var pSpecs) ? pSpecs : new(),
            }).ToList()
        };

        return Ok(result);
    }
}