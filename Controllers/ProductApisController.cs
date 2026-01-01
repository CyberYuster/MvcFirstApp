using Microsoft.AspNetCore.Mvc;
using MvcDatabaseApp.Models.ViewModels;
using MvcDatabaseApp.Services.Contracts;


namespace MvcDatabaseApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApisController : Controller
    {
        private readonly IProductService _productService;

        public ProductApisController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _productService.GetActiveProductsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _productService.GetProductByIdAsync(id));
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        => Ok(await _productService.CreateProductAsync(model));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductViewModel model)
            => Ok(await _productService.UpdateProductAsync(id, model));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _productService.DeleteProductAsync(id));
    }
}
