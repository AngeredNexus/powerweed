namespace WeedDelivery.Backend.Ecosystem.Store
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            return products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ThcContent = product.ThcContent,
                CategoryId = product.CategoryId,
                ImageUrl = product.Images?.FirstOrDefault()?.Url
            }).ToList();
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ThcContent = product.ThcContent,
                CategoryId = product.CategoryId,
                ImageUrl = product.Images?.FirstOrDefault()?.Url
            };
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepo.GetByCategoryAsync(categoryId);
            return products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ThcContent = product.ThcContent,
                CategoryId = product.CategoryId,
                ImageUrl = product.Images?.FirstOrDefault()?.Url
            }).ToList();
        }
    }
}
