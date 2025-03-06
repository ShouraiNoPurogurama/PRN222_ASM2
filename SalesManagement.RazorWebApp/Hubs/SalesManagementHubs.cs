using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SalesManagement.RazorWebApp.Events;
using SalesManagement.Repositories.Models;
using SalesManagement.Repository.Dtos;
using SalesManagement.Repository.Models;
using SalesManagement.Service;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SalesManagement.RazorWebApp.Hubs;

public class SalesManagementHubs : Hub
{
    private readonly IProductService _productService;
    private readonly CategoryService _categoryService;
    private readonly OutboxService _outboxService;

    public SalesManagementHubs(IProductService productService, CategoryService categoryService, OutboxService outboxService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _outboxService = outboxService;
    }

    public async Task SendCreatedProduct(string productJson)
    {
        try
        {
            var productDto = JsonConvert.DeserializeObject<ProductDto>(productJson)
                             ?? throw new InvalidDataException();

            var validationResponse = await _productService.CreateAsync(productDto);

            if (!validationResponse.IsValid)
            {
                await Clients.Caller.SendAsync("Receive_ValidationErrors", validationResponse.Errors);
                return;
            }

            var product = productDto.Adapt<Product>();

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
            var productDto = JsonConvert.DeserializeObject<ProductDto>(productJson)
                             ?? throw new InvalidDataException();

            var oldProduct = await _productService.GetByIdAsync(productDto.Id!.Value);
            
            var validationResponse = await _productService.UpdateAsync(productDto);

            if (!validationResponse.IsValid)
            {
                await Clients.Caller.SendAsync("Receive_ValidationErrors", validationResponse.Errors);
                return;
            }

            var newProduct = await _productService.GetByIdAsync(productDto.Id!.Value);

            if (oldProduct!.Price != newProduct!.Price)
            {
                var eventMessage = newProduct.Adapt<ProductPriceChangedIntegrationEvent>();

                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    Type = typeof(ProductPriceChangedIntegrationEvent).AssemblyQualifiedName!,
                    Content = JsonSerializer.Serialize(eventMessage),
                    OccuredOn = DateTime.UtcNow
                };

                await _outboxService.CreateAsync(outboxMessage);
            }

            await Clients.All.SendAsync("Receive_UpdatedProduct", newProduct);
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

            Console.WriteLine("Product id: " + productId);

            await Clients.All.SendAsync("Receive_DeletedProduct", productId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}