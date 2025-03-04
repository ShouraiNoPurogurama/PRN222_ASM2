using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesManagement.Repositories.DBContext;
using SalesManagement.Repositories.Models;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Pages.Products;

public class CreateModel : PageModel
{
    private readonly IProductService _productService;
    private readonly CategoryService _categoryService;

    public CreateModel(IProductService productService, CategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        var categories = await _categoryService.GetAllAsync();
        ViewData["CategoryList"] = new SelectList(categories, "Id", "Name");
        
        return Page();
    }

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        await _productService.CreateAsync(Product);

        return RedirectToPage("./Index");
    }
}