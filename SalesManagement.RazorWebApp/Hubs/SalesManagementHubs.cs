using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SalesManagement.Repositories.Models;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Hubs;

public class SalesManagementHubs : Hub
{
    private readonly IProductService _productService;
    private readonly CategoryService _categoryService;

    public SalesManagementHubs(IProductService productService, CategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task SendCreatedProduct(string productJson)
    {
        try
        {
            var product = JsonConvert.DeserializeObject<Product>(productJson)
                ?? throw new InvalidDataException();

            var validationResponse = await _productService.CreateAsync(product);

            if (!validationResponse.IsValid)
            {
                await Clients.Caller.SendAsync("Receive_ValidationErrors", validationResponse.Errors);
                return;
            }
            
            var category = await _categoryService.GetByIdAsync(product.CategoryId.Value);
            
            product.Category = category;

            await Clients.All.SendAsync("Receive_CreatedProduct", product);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
        public async Task SendUpdatedProduct(string productJson)
    {
        try
        {
            var product = JsonConvert.DeserializeObject<Product>(productJson)
                ?? throw new InvalidDataException();
            
            var validationResponse = await _productService.UpdateAsync(product);
            
            if (!validationResponse.IsValid)
            {
                await Clients.Caller.SendAsync("Receive_ValidationErrors", validationResponse.Errors);
                return;
            }
            
            await Clients.All.SendAsync("Receive_UpdatedProduct", product);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task SendDeletedProduct(string productId)
    {
        try
        {
            await _productService.DeleteAsync(Guid.Parse(productId));

            Console.WriteLine("Product id: " +productId);
            
            await Clients.All.SendAsync("Receive_DeletedProduct", productId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}