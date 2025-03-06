using Discount.gRPC;
using Mapster;
using SalesManagement.Repositories.Models;
using SalesManagement.Repository.Dtos;
using SalesManagement.Repository.Pagination;

namespace SalesManagement.Service;

public interface IProductService
{
    Task<PaginatedResult<GetProductDto>> GetAllAsync(PaginationRequest? paginationRequest);
    Task<GetProductDto?> GetByIdAsync(Guid id);
    Task<ValidationResponse> CreateAsync(ProductDto productDto);
    Task<ValidationResponse> UpdateAsync(ProductDto product);
    Task<bool> DeleteAsync(Guid id);

    Task<PaginatedResult<GetProductDto>> Search(PaginationRequest paginationRequest, string? name, string? category,
        string? ingredients);
}

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ValidationService _validationService;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

    public ProductService(IUnitOfWork unitOfWork, ValidationService validationService,
        DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _discountProtoService = discountProtoService;
    }

    public async Task<PaginatedResult<GetProductDto>> GetAllAsync(PaginationRequest? paginationRequest)
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync(paginationRequest);

        var productDtos = new List<GetProductDto>();

        foreach (var product in products.Data)
        {
            var coupon = await _discountProtoService.GetDiscountAsync(new GetDiscountRequest
            {
                ProductId = product.Id.ToString()
            });

            if (coupon is not null)
            {
                var productDto = product.Adapt<GetProductDto>() with
                {
                    CouponDto = coupon.Adapt<CouponDto>()
                };
                productDtos.Add(productDto);
            }
            else
            {
                productDtos.Add(product.Adapt<GetProductDto>());
            }
        }

        return new PaginatedResult<GetProductDto>(
            products.PageIndex,
            products.PageSize,
            products.Count,
            productDtos);
    }

    public async Task<GetProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

        var coupon = await _discountProtoService.GetDiscountAsync(new GetDiscountRequest()
        {
            ProductId = product?.Id.ToString()
        });

        if (coupon is not null)
        {
            return product.Adapt<GetProductDto>() with
            {
                CouponDto = coupon.Adapt<CouponDto>()
            };
        }

        return product.Adapt<GetProductDto>();
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

    public async Task<PaginatedResult<GetProductDto>> Search(PaginationRequest paginationRequest, string? name, string? category,
        string? ingredients)
    {

        var paginatedResult =
            await _unitOfWork.ProductRepository.FindByConditionAsync(name, category, ingredients, paginationRequest);

        var dataResult = new List<GetProductDto>();

        foreach (var product in paginatedResult.Data)
        {
            var coupon = await _discountProtoService.GetDiscountAsync(new GetDiscountRequest()
            {
                ProductId = product.Id.ToString()
            });

            if (coupon is not null)
            {
                dataResult.Add(product.Adapt<GetProductDto>() with
                {
                    CouponDto = coupon.Adapt<CouponDto>()
                });
            }
            else
            {
                dataResult.Add(product.Adapt<GetProductDto>());
            }
        }

        return new PaginatedResult<GetProductDto>(
            paginatedResult.PageIndex,
            paginatedResult.PageSize,
            paginatedResult.Count,
            dataResult);
    }

    public Dictionary<string, List<string>> GetValidationErrors()
    {
        return _validationService.GetValidationErrors();
    }
}