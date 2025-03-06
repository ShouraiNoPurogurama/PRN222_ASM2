namespace SalesManagement.Repository.Dtos;

public record CouponDto(
    string Id,
    string ProductId,
    string Description,
    int Amount);