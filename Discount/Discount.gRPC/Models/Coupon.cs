namespace Discount.gRPC.Models;

public class Coupon
{
    public string Id { get; set; }
    public string ProductId { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
}