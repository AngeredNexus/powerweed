using Microsoft.AspNetCore.Mvc;
using WeedDelivery.Backend.App.Market.Customer.Interfaces;
using WeedDelivery.Backend.Models.Api.Response;

namespace WeedDelivery.Backend.Api.v1.Market;

[ApiVersion("1")]
[Microsoft.AspNetCore.Mvc.Route("api/v{version:apiVersion}/store/")]
[ApiController]
[Tags("Customer API")]
public class MarketCustomerController : Controller
{

    // private readonly IMarketCustomerCategoryService _categoryService;
    private readonly IMarketCustomerOrderService _orderService;
    private readonly IMarketCustomerSearchService _searchService;

    public MarketCustomerController(IMarketCustomerOrderService orderService, IMarketCustomerSearchService searchService)
    {
        _orderService = orderService;
        _searchService = searchService;
    }

    [HttpGet("tree-of-choice")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> GetCategoriesTree()
    {
        // var resultTree = await _categoryService.GetCategoryTree();
        // var response = new ApiWeedTreeResponse()
        // {
        //     Tree = resultTree
        // };
        //
        // return new OkObjectResult(response);
        return null;
    }

    // Модель на выход - доменная 
    [HttpGet("search")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> SearchByCategory([FromQuery] int main, [FromQuery] int branch)
    {
        var searchResult = await _searchService.SearchByCategory();
        var response = new ApiWeedSearchResponse()
        {
            Items = searchResult
        };
        
        return new OkObjectResult(response);
    }

    // Модель на выход - доменная 
    [HttpGet("search-all")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> SearchAll()
    {
        var searchResult = await _searchService.SearchByCategory();
        var response = new ApiWeedSearchResponse()
        {
            Items = searchResult
        };
        
        return new OkObjectResult(response);
    }
    
    // Вход, выход - вход доменная, выход http
    [HttpPost("order")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> CreateOrder([FromBody] WeedDatabase.Domain.App.Order order)
    {
        try
        {
            await _orderService.TryToPlaceOrder(order);
        }
        catch (Exception ex)
        {
            return new BadRequestResult();
        }

        return Ok();
    }
}