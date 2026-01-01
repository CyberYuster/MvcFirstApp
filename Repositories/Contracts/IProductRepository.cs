using MvcDatabaseApp.Models;

namespace MvcDatabaseApp.Repositories.Contracts
{
    public interface IProductRepository:IRepository<Product>
    {
        // Product-specific methods
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetProductsInPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<Product> GetProductWithDetailsAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<bool> ProductNameExistsAsync(string name, int? excludeId = null);
        new void Update(Product entity);
    }
}
