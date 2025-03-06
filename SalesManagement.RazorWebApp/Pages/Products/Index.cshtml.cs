using SalesManagement.Repository.Dtos;
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

    public PaginatedResult<GetProductDto> PaginatedProducts { get; set; }
    public string? SearchName { get; set; }
    public string? SearchIngredients { get; set; }
    public string? SearchCategory { get; set; }

    public async Task OnGetAsync(string? name, string? ingredients, string? category, int pageIndex = 0, int pageSize = 5)
    {
        SearchName = name;
        SearchIngredients = ingredients;
        SearchCategory = category;

        var paginationRequest = new PaginationRequest
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        };

        var result = await _productService.Search(paginationRequest, name, category, ingredients);

        PaginatedProducts = result;
    }
}