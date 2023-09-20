using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeedDatabase.Domain.App;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Models.Api.Common;
using WeedDelivery.Backend.Models.Api.Request;
using WeedDelivery.Backend.Models.Api.Response;
using WeedDelivery.Backend.Systems.App.Market.Customer.Interfaces;

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
    
    private readonly IUserRepository _userRepository;

    public MarketCustomerController(IMarketCustomerOrderService orderService, IMarketCustomerSearchService searchService, IUserRepository userRepository)
    {
        _orderService = orderService;
        _searchService = searchService;
        _userRepository = userRepository;
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
    public async Task<IActionResult> CreateOrder([FromBody] OrderApi order)
    {
        try
        {
            // replace with validation service or autofac
            var coreOrder = new WeedDatabase.Domain.App.Order()
            {
                Firstname = order.Firstname,
                Lastname = order.Lastname,
                PhoneNumber = order.PhoneNumber,
                Address = order.Address,
                Comment = order.Comment,
                Items = order.Items.Select(x => new OrderItem()
                {
                    WeedId = x.WeedId,
                    Amount = x.Amount
                }).ToList()
            };

            var sysUser = await _userRepository.GetUserByIdentityHash(order.Hash);
            
            if(sysUser is not null)
            {
                coreOrder.AppUserId = sysUser.Id;
                await _orderService.TryToPlaceOrder(coreOrder, sysUser);
                return Ok();
            }
            
            return BadRequest();
            
        }
        catch (Exception ex)
        {
            // TODO LOG ERRORS MFUCKER
            return BadRequest();
        }
    }
}