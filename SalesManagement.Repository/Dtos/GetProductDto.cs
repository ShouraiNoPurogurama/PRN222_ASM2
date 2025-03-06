using SalesManagement.Repositories.Models;

namespace SalesManagement.Repository.Dtos;

public record GetProductDto(
    Guid Id,
    string Name,
    Guid CategoryId,
    Category Category,
    string Description,
    string? ImageFile,
    decimal Price,
    string Ingredients,
    string UsageInstructions,
    int StockQuantity,
    DateTimeOffset? CreatedAt,
    string CreatedBy,
    DateTimeOffset? LastModified,
    string LastModifiedBy,
    CouponDto? CouponDto
    );