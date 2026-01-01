using MvcDatabaseApp.Models.ViewModels;

namespace MvcDatabaseApp.Services.Contracts
{
    public interface IProductService
    {
        // CRUD Operations
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<ProductViewModel> GetProductByIdAsync(int id);
        Task<ProductViewModel> CreateProductAsync(ProductViewModel productVm);
        Task<ProductViewModel> UpdateProductAsync(int id, ProductViewModel productVm);
        Task<bool> DeleteProductAsync(int id);

        // Business Logic Operations
        Task<IEnumerable<ProductViewModel>> GetActiveProductsAsync();
        Task<IEnumerable<ProductViewModel>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<string>> GetProductCategoriesAsync();
        Task<bool> ProductExistsAsync(int id);
        Task<bool> ProductNameExistsAsync(string name, int? excludeId = null);

        // Reports/Statistics
        Task<decimal> GetTotalInventoryValueAsync();
        Task<int> GetActiveProductCountAsync();
        Task<Dictionary<string, int>> GetProductCountByCategoryAsync();
    }
}
