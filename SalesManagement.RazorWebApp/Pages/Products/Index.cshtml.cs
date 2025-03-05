using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesManagement.Repositories.Models;
using SalesManagement.Repository.Pagination;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Pages.Products;

public class IndexModel : PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    public PaginatedResult<Product> PaginatedProducts { get; set; } = default!;

    public async Task OnGetAsync(int pageIndex = 0, int pageSize = 5)
    {
        var paginationRequest = new PaginationRequest(pageIndex, pageSize);
        PaginatedProducts = await _productService.GetAllAsync(paginationRequest);
    }
}