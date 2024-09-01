namespace WeedDelivery.Backend.Ecosystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Images).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)
        {
            return await _context.Products.Include(p => p.Images).Where(p => p.CategoryId == categoryId).ToListAsync();
        }
    }
}
