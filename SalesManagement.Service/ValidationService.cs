using SalesManagement.Repositories.Models;
using SalesManagement.Repository.Common;
using SalesManagement.Repository.Dtos;

namespace SalesManagement.Service;

public class ValidationService
{
    private readonly ModelStateDictionary _modelState = new ModelStateDictionary();

    public bool ValidateProduct(ProductDto product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            _modelState.AddModelError("Product.Name", "Name is required.");
        }

        if (product.CategoryId is null || product.CategoryId == Guid.Empty)
        {
            _modelState.AddModelError("Product.CategoryId", "Category is required.");
        }

        if (product.Price is null or <= 0)
        {
            _modelState.AddModelError("Product.Price", "Price must be greater than zero.");
        }
        
        if (string.IsNullOrWhiteSpace(product.Description))
        {
            _modelState.AddModelError("Product.Description", "Description is required.");
        }

        if (string.IsNullOrWhiteSpace(product.Ingredients))
        {
            _modelState.AddModelError("Product.Ingredients", "Ingredients are required.");
        }

        if (string.IsNullOrWhiteSpace(product.UsageInstructions))
        {
            _modelState.AddModelError("Product.UsageInstructions", "Usage instructions are required.");
        }

        if (product.StockQuantity is null or < 0)
        {
            _modelState.AddModelError("Product.StockQuantity", "Stock quantity cannot be negative.");
        }

        return _modelState.IsValid;
    }

    public Dictionary<string, List<string>> GetValidationErrors() => _modelState.Errors;
}