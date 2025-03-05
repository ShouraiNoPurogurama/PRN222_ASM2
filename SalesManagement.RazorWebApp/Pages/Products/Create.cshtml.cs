using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesManagement.Repositories.Models;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Pages.Products;

public class CreateModel : PageModel
{
    private readonly IProductService _productService;
    private readonly CategoryService _categoryService;
    private readonly IWebHostEnvironment _hostEnvironment;

    public CreateModel(IProductService productService, CategoryService categoryService, IWebHostEnvironment hostEnvironment)
    {
        _productService = productService;
        _categoryService = categoryService;
        _hostEnvironment = hostEnvironment;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    // [FileExtensions(Extensions="png,jpg,jpeg,gif")]
    public IFormFile? FileUpload { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var categories = await _categoryService.GetAllAsync();
        ViewData["CategoryList"] = new SelectList(categories, "Id", "Name");

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

        if (FileUpload != null)
        {
            var uniqueFileName = await SaveImageAsync(FileUpload);
            Product.ImageFile = uniqueFileName;
        }

        await _productService.CreateAsync(Product);

        return RedirectToPage("./Index");
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