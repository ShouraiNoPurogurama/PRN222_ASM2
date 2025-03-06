using Mapster;
using SalesManagement.Repositories.Models;
using SalesManagement.Repository.Dtos;
using SalesManagement.Repository.Pagination;

namespace SalesManagement.Service;

public interface IProductService
{
    Task<PaginatedResult<Product>> GetAllAsync(PaginationRequest? paginationRequest);
    Task<Product?> GetByIdAsync(Guid id);
    Task<ValidationResponse> CreateAsync(ProductDto productDto);
    Task<ValidationResponse> UpdateAsync(ProductDto product);
    Task<bool> DeleteAsync(Guid id);

    Task<PaginatedResult<Product>> Search(PaginationRequest paginationRequest, string? name, string? category,
        string? ingredients);
}

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ValidationService _validationService;

    public ProductService(IUnitOfWork unitOfWork, ValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
    }

    public async Task<PaginatedResult<Product>> GetAllAsync(PaginationRequest? paginationRequest)
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync(paginationRequest);

        return products;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.ProductRepository.GetByIdAsync(id);
    }


    public async Task<ValidationResponse> CreateAsync(ProductDto productDto)
    {
        if (!_validationService.ValidateProduct(productDto))
        {
            return new ValidationResponse(false, _validationService.GetValidationErrors());
        }

        var product = productDto.Adapt<Product>();
        
        await _unitOfWork.ProductRepository.CreateAsync(product);
        return new ValidationResponse(true, new Dictionary<string, List<string>>());
    }

    public async Task<ValidationResponse> UpdateAsync(ProductDto productDto)
    {
        if (!_validationService.ValidateProduct(productDto))
        {
            return new ValidationResponse(false, _validationService.GetValidationErrors());
        }

        await _unitOfWork.ProductRepository.UpdateAsync(productDto.Adapt<Product>());
        return new ValidationResponse(true, new Dictionary<string, List<string>>());
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        return await _unitOfWork.ProductRepository.RemoveAsync(product);
    }

    public async Task<PaginatedResult<Product>> Search(PaginationRequest paginationRequest, string? name, string? category,
        string? ingredients)
    {
        var paginatedResult = await _unitOfWork.ProductRepository.FindByConditionAsync(p =>
            (string.IsNullOrEmpty(name) || p.Name.Contains(name)) &&
            (string.IsNullOrEmpty(category) || p.Category.Name.Contains(category)) &&
            (string.IsNullOrEmpty(ingredients) || p.Ingredients.Contains(ingredients)), paginationRequest);

        return paginatedResult;
    }

    public Dictionary<string, List<string>> GetValidationErrors()
    {
        return _validationService.GetValidationErrors();
    }
}