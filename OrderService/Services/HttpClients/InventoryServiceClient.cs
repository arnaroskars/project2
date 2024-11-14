using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OrderService.Services.HttpClients.Interfaces;
using OrderService.Models;

public class InventoryServiceClient : IInventoryServiceClient
{
    private readonly HttpClient _httpClient;

    public InventoryServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int productId)
    {
        var response = await _httpClient.GetAsync($"api/products/{productId}");
        if (response.IsSuccessStatusCode)
        {
            var productJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Received JSON: {productJson}"); // Log the raw JSON

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var product = JsonSerializer.Deserialize<ProductDto>(productJson, options);
            Console.WriteLine($"Deserialized Product: Price={product?.price}, Name={product?.productName}");
            return product;
        }

        Console.WriteLine($"response from GetProductById: {response}");
        return null;
    }

}
