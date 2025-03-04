using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Repositories.DBContext;
using SalesManagement.Repositories.Models;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Pages.Products;

public class EditModel : PageModel
{
    private readonly IProductService _productService;
    private readonly CategoryService _categoryService;

    public EditModel(IProductService productService, CategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

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

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var existingProduct = await _productService.GetByIdAsync(Product.Id);
        
        if (existingProduct is null)
        {
            return NotFound();
        }

        try
        {
            await _productService.UpdateAsync(Product);

        }
        catch (DbUpdateConcurrencyException e)
        {
            Console.Write(e.Message);
        }

        return RedirectToPage("./Index");
    }
}