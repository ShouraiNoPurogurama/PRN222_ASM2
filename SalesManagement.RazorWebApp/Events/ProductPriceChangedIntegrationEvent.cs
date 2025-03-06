namespace SalesManagement.RazorWebApp.Events;

public record ProductPriceChangedIntegrationEvent(
    Guid Id,
    string Name,
    Guid CategoryId,
    string Description,
    decimal Price,
    string Ingredients,
    string UsageInstructions,
    int StockQuantity,
    DateTimeOffset? CreatedAt,
    string CreatedBy,
    DateTimeOffset? LastModified,
    string LastModifiedBy
);