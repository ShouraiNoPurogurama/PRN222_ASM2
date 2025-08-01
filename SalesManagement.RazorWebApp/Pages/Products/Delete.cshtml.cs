using Microsoft.EntityFrameworkCore;
using SalesManagement.Repositories.DBContext;
using SalesManagement.Repositories.Models;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Pages.Products;

[Authorize(Policy = "AdminOrManagerOnly")]
public class DeleteModel : PageModel
{
    private readonly IProductService _productService;
    public DeleteModel(SalesManagementDBContext context, IProductService productService)
    {
        _productService = productService;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value);

        if (product == null)
            return NotFound();
        Product = product.Adapt<Product>();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value);
        
        if (product != null)
        {
            Product = product.Adapt<Product>();
            await _productService.DeleteAsync(Product.Id);
        }
        
        return RedirectToPage("./Index");
    }
}