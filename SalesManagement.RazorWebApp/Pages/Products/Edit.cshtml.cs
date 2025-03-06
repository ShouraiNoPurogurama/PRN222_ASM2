using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Repositories.Models;
using SalesManagement.Repository.Dtos;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Pages.Products;

[IgnoreAntiforgeryToken(Order = 2000)] //To be able to take requests from Js scripts
public class EditModel : PageModel
{
    private readonly IProductService _productService;
    private readonly CategoryService _categoryService;
    private readonly IWebHostEnvironment _hostEnvironment;

    public EditModel(IProductService productService, CategoryService categoryService, IWebHostEnvironment hostEnvironment)
    {
        _productService = productService;
        _categoryService = categoryService;
        _hostEnvironment = hostEnvironment;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    [BindProperty]
    public IFormFile? FileUpload { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value);
        if (product == null) return NotFound();
        Product = product;

        var categories = await _categoryService.GetAllAsync();
        ViewData["CategoryList"] = new SelectList(categories, "Id", "Name", Product.Category);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ValidateProduct())
        {
            var categories = await _categoryService.GetAllAsync();
            ViewData["CategoryList"] = new SelectList(categories, "Id", "Name");
            return Page();
        }

        var existingProduct = await _productService.GetByIdAsync(Product.Id);
        if (existingProduct is null) return NotFound();

        if (FileUpload != null)
        {
            var uniqueFileName = await SaveImageAsync(FileUpload);
            Product.ImageFile = uniqueFileName;
        }

        await _productService.UpdateAsync(Product.Adapt<ProductDto>());

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostUploadImageAsync(IFormFile? imageFile)
    {
        if (imageFile.Length == 0)
        {
            return BadRequest();
        }

        var uniqueFileName = await SaveImageAsync(imageFile);
        return Content(uniqueFileName);
    }
    
    private async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images/products");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return uniqueFileName;
    }

    private bool ValidateProduct()
    {
        if (string.IsNullOrWhiteSpace(Product.Name))
        {
            ModelState.AddModelError("Product.Name", "Name is required.");
        }

        if (Product.CategoryId == Guid.Empty)
        {
            ModelState.AddModelError("Product.CategoryId", "Category is required.");
        }

        if (Product.Price <= 0)
        {
            ModelState.AddModelError("Product.Price", "Price must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(Product.Description))
        {
            ModelState.AddModelError("Product.Description", "Description is required.");
        }

        if (string.IsNullOrWhiteSpace(Product.Ingredients))
        {
            ModelState.AddModelError("Product.Ingredients", "Ingredients are required.");
        }

        if (string.IsNullOrWhiteSpace(Product.UsageInstructions))
        {
            ModelState.AddModelError("Product.UsageInstructions", "Usage instructions are required.");
        }

        if (Product.StockQuantity < 0)
        {
            ModelState.AddModelError("Product.StockQuantity", "Stock quantity cannot be negative.");
        }

        return ModelState.IsValid;
    }
}