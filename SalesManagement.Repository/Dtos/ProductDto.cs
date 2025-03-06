namespace SalesManagement.Repository.Dtos;

public record ProductDto(
    Guid? Id,
    string? Name,
    Guid? CategoryId,
    string? Description,
    string? ImageFile,
    decimal? Price,
    string? Ingredients,
    string? UsageInstructions,
    int? StockQuantity,
    DateTimeOffset? CreatedAt,
    string? CreatedBy,
    DateTimeOffset? LastModified,
    string? LastModifiedBy);