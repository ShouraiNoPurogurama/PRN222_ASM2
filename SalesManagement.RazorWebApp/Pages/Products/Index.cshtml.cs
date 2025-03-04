using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesManagement.Repositories.Models;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Pages.Products;

public class IndexModel : PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    public IList<Product> Product { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Product = (await _productService.GetAllAsync()).ToList();
    }
}