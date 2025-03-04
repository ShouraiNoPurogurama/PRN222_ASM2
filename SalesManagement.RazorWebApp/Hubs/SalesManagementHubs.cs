using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SalesManagement.Repositories.Models;
using SalesManagement.Service;

namespace SalesManagement.RazorWebApp.Hubs;

public class SalesManagementHubs : Hub
{
    private readonly IProductService _productService;

    public SalesManagementHubs(IProductService productService)
    {
        _productService = productService;
    }

    public async Task SendCreatedProduct(string productJson)
    {
        try
        {
            var product = JsonConvert.DeserializeObject<Product>(productJson)
                ?? throw new InvalidDataException();

            await Clients.All.SendAsync("Receive_CreatedProduct", product);
            
            Console.WriteLine(product.Category);

            await _productService.CreateAsync(product);
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