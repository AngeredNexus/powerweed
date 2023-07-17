using Microsoft.AspNetCore.Mvc;
using WeedDatabase.Domain;
using WeedDelivery.Backend.App.Market.Customer;
using WeedDelivery.Backend.App.Market.Customer.Interfaces;
using WeedDelivery.Backend.Models.Api.Response;

namespace WeedDelivery.Backend.Api.v1.Market;

public class MarketCustomerController : Controller
{

    private readonly IMarketCustomerCategoryService _categoryService;
    private readonly IMarketCustomerOrderService _orderService;
    private readonly IMarketCustomerSearchService _searchService;

    public MarketCustomerController(IMarketCustomerCategoryService categoryService, IMarketCustomerOrderService orderService, IMarketCustomerSearchService searchService)
    {
        _categoryService = categoryService;
        _orderService = orderService;
        _searchService = searchService;
    }

    [HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("/tree-of-choice")]
    public async Task<IActionResult> GetCategoriesTree()
    {
        var resultTree = await _categoryService.GetCategoryTree();
        var response = new ApiWeedTreeResponse()
        {
            Tree = resultTree
        };
        
        return new OkObjectResult(response);
    }

    // Модель на выход - доменная 
    [HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("/search-by-tree")]
    public async Task<IActionResult> SearchByCategory([FromQuery] int main, [FromQuery] int branch)
    {
        var searchResult = await _searchService.SearchByCategory();
        var response = new ApiWeedSearchResponse()
        {
            Items = searchResult
        };
        
        return new OkObjectResult(response);
    }

    // Вход, выход - вход доменная, выход http
    [HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("/order")]
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