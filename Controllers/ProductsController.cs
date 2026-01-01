using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcDatabaseApp.Data;
using MvcDatabaseApp.Models;
using MvcDatabaseApp.Models.ViewModels;
using MvcDatabaseApp.Services.Contracts;

namespace MvcDatabaseApp.Controllers
{
   
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService ??throw new ArgumentNullException(nameof(productService));
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
        }
        //GET : products
        [HttpGet]
        public async Task<IActionResult> Index()  // Parameterless version
        {
            return await Index(null);  // Call the main version with null
        }

        [HttpGet]
        [Route("Products/Search")]
        public async Task<IActionResult> Index(string searchTerm)
        {
            try
            {
                ViewBag.SearchTerm = searchTerm;

                var products = string.IsNullOrWhiteSpace(searchTerm)
                    ? await _productService.GetActiveProductsAsync()
                    : await _productService.SearchProductsAsync(searchTerm);
                // Get statistics for display
                ViewBag.TotalProducts = await _productService.GetActiveProductCountAsync();
                ViewBag.TotalValue = await _productService.GetTotalInventoryValueAsync();

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                TempData["ErrorMessage"] = "An error occurred while retrieving products.";
                return View(Enumerable.Empty<ProductViewModel>());
            }
           
        }
        //GET : Products/Details/[number]
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
            
                var products = await _productService.GetProductByIdAsync(id);
                if (products == null)
                {
                    TempData["ErrorMessage"] = "Product not found.";
                    return RedirectToAction(nameof(Index));
                   
                }
                return View(products);
            }
            catch (Exception ex)
            {
            _logger.LogError($"{ex.Message}", $"Error or displaying Details with id {id}");
                TempData["ErrorMessage"] = "An error occurred while retrieving the product.";
                return RedirectToAction(nameof(Index));
            }
            
           
        }
        //GET:products/create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await PopulateCategoriesDropdown();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ ex.Message}","Error on retrieving the Create Form");
                TempData["ErrorMessage"] = "An Error on loading the form";
                return RedirectToAction(nameof(Index));
            }
        }

       

        //POST :Products/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("Id,Name,Price,Description,isActive")]*/ ProductViewModel productVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateCategoriesDropdown();
                    return View(productVm);
                }
                var createdProduct = await _productService.CreateProductAsync(productVm);

                TempData["SuccessMessage"] = $"Product '{createdProduct.Name}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                await PopulateCategoriesDropdown();
                return View(productVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                TempData["ErrorMessage"] = "An error occurred while creating the product.";
                await PopulateCategoriesDropdown();
                return View(productVm);
            }
        }
        //GET:Products/Edit/[number]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var products = await _productService.GetProductByIdAsync(id);
                if (products == null)
                {
                    TempData["ErrorMessage"] = "Product not found.";
                    return RedirectToAction(nameof(Index));
                }
                await PopulateCategoriesDropdown();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", $"Error loading edit form for product ID {id}");
                TempData["ErrorMessage"] = "An error occurred while loading the edit form.";
                return RedirectToAction(nameof(Index));
            }
            
        }
        //POST: Products/Edit/[number]
        //[HttpPost]
        [HttpPut("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, /*[Bind("Id,Name,Price,Description,isActive,CreatedDate")]*/ ProductViewModel productVm)
        {
            try
            {
                if (id != productVm.Id)
                {
                    TempData["ErrorMessage"] = "Product ID mismatch.";
                    return RedirectToAction(nameof(Index));
                }
                if (!ModelState.IsValid)
                {
                    await PopulateCategoriesDropdown();
                    return View(productVm);
                }
                var updatedProduct = await _productService.UpdateProductAsync(id, productVm);

                TempData["SuccessMessage"] = $"Product '{updatedProduct.Name}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Product with ID {id} not found for update");
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                await PopulateCategoriesDropdown();
                return View(productVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product with ID {id}");
                TempData["ErrorMessage"] = "An error occurred while updating the product.";
                await PopulateCategoriesDropdown();
                return View(productVm);
            }
        }
        [HttpGet]
        public async Task <IActionResult> Statistics()
        {
            try
            {
               
                // Or if using ViewBag, ensure properties are set
                ViewBag.ActiveCount = await _productService.GetActiveProductCountAsync();
                ViewBag.TotalValue = await _productService.GetTotalInventoryValueAsync();
                ViewBag.Categories = await _productService.GetProductCategoriesAsync();
                ViewBag.CategoryStats = await _productService.GetProductCountByCategoryAsync();
                // etc...

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving statistics");
                TempData["ErrorMessage"] = "An error occurred while retrieving statistics.";
                return RedirectToAction("Index"); // This might cause the issue if the view expects data
            }
        }

        //GET: Products/Delete/[number]
        [HttpGet("Delete/{id}")]
       public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Product Not Found";
                    return RedirectToAction(nameof(Index));
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", $"Error loading delete confirmation for product ID {id}");
                TempData["ErrorMessage"] = "An Error Occured";
                return RedirectToAction(nameof(Index));
            }
        }
        //POST: Products/Delete/[number]
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult>DeleteConfirmed(int id)
        {
            try
            {
                var success = await _productService.DeleteProductAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Product deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Product not found.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product with ID {id}");
                TempData["ErrorMessage"] = "An error occurred while deleting the product.";
                return RedirectToAction(nameof(Index));
            }
            
        }
        

        private async Task PopulateCategoriesDropdown()
        {
            var categories = await _productService.GetProductCategoriesAsync();
            ViewBag.Categories = new SelectList(categories);
        }

    }
}
