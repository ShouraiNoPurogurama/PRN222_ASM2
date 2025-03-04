using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Repositories.DBContext;
using SalesManagement.Repositories.Models;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Pages.Products;

public class DetailsModel : PageModel
{
    private readonly IProductService _productService;

    public DetailsModel(IProductService productService)
    {
        _productService = productService;
    }

    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value);
        if (product == null)
            return NotFound();
        Product = product;
        return Page();
    }
}