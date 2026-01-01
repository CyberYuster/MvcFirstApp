using AutoMapper;
using MvcDatabaseApp.Models;
using MvcDatabaseApp.Models.ViewModels;
using MvcDatabaseApp.Repositories.Contracts;
using MvcDatabaseApp.Services.Contracts;

namespace MvcDatabaseApp.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductViewModel> CreateProductAsync(ProductViewModel productVm)
        {
            // Validate business rules
            if (await _productRepository.ProductNameExistsAsync(productVm.Name))
                throw new InvalidOperationException($"Product with name '{productVm.Name}' already exists.");

            // Map ViewModel to Entity
            var product = _mapper.Map<Product>(productVm);

            // Set audit fields
            product.CreateDate = DateTime.UtcNow;
            product.LastModified = DateTime.UtcNow;

            // Add to repository
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            // Return created product
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return false;

            // Soft delete (set inactive)
            product.isActive = false;
            product.LastModified = DateTime.UtcNow;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetActiveProductCountAsync()
        {
            return await _productRepository.CountAsync(p => p.isActive);
        }

        public async Task<IEnumerable<ProductViewModel>> GetActiveProductsAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return _mapper.Map<IEnumerable<ProductViewModel>>(products);
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<IEnumerable<string>> GetProductCategoriesAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products
                .Where(p => !string.IsNullOrEmpty(p.Category))
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        public async Task<Dictionary<string, int>> GetProductCountByCategoryAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products
                .Where(p => p.isActive && !string.IsNullOrEmpty(p.Category))
                .GroupBy(p => p.Category)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<decimal> GetTotalInventoryValueAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return products.Sum(p => p.Price);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _productRepository.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> ProductNameExistsAsync(string name, int? excludeId = null)
        {
            return await _productRepository.ProductNameExistsAsync(name, excludeId);
        }

        public async Task<IEnumerable<ProductViewModel>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.SearchProductsAsync(searchTerm);
            return _mapper.Map<IEnumerable<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> UpdateProductAsync(int id, ProductViewModel productVm)
        {
            if (id != productVm.Id)
                throw new ArgumentException("ID mismatch");

            // Check if product exists
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            // Validate business rules
            if (await _productRepository.ProductNameExistsAsync(productVm.Name, id))
                throw new InvalidOperationException($"Product with name '{productVm.Name}' already exists.");

            // Update entity
            _mapper.Map(productVm, existingProduct);
            existingProduct.LastModified = DateTime.UtcNow;

            // Save changes
            _productRepository.Update(existingProduct);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<ProductViewModel>(existingProduct);
        }
    }
}
