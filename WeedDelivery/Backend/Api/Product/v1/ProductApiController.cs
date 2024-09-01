namespace WeedDelivery.Backend.Api.Product.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Microsoft.AspNetCore.Mvc.Route("api/{version}/products")]
    public class ProductApiController : Controller
    {
        private readonly IProductService _productService;

        public ProductApiController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }
    }
}
