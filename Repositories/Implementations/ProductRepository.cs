using Microsoft.EntityFrameworkCore;
using MvcDatabaseApp.Data;
using MvcDatabaseApp.Models;
using MvcDatabaseApp.Repositories.Contracts;

namespace MvcDatabaseApp.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _dbSet.Where(p => p.isActive).OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _dbSet.Where(p=>p.Category==category&& p.isActive).OrderBy(p=>p.Name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsInPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.isActive)
                .OrderBy(p => p.Price)
                .ToListAsync();
        }

        public async Task<Product> GetProductWithDetailsAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ProductNameExistsAsync(string name, int? excludeId = null)
        {
            var query = _dbSet.Where(p => p.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetActiveProductsAsync();

            searchTerm = searchTerm.ToLower();

            return await _dbSet
                .Where(p => p.isActive &&
                    (p.Name.ToLower().Contains(searchTerm) ||
                     p.Description.ToLower().Contains(searchTerm) ||
                     p.Category.ToLower().Contains(searchTerm)))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        // Override AddAsync to set CreatedDate
        public override async Task AddAsync(Product entity)
        {
            if (entity.CreateDate == default)
                entity.CreateDate = DateTime.UtcNow;

            entity.LastModified = DateTime.UtcNow;

            await base.AddAsync(entity);
        }
        // Override Update to set LastModified
        public override void Update(Product entity)
        {
            entity.LastModified = DateTime.UtcNow;
            base.Update(entity);
        }
    }
}
