using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OrderService.Services.HttpClients.Interfaces;
using OrderService.Models;

public class BuyerServiceClient : IBuyerServiceClient
{
    private readonly HttpClient _httpClient;

    public BuyerServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BuyerDto?> GetBuyerByIdAsync(int buyerId)
    {
        var response = await _httpClient.GetAsync($"api/buyers/{buyerId}");
        if (response.IsSuccessStatusCode)
        {
            var buyerJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Received JSON from BuyerService: {buyerJson}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var buyer = JsonSerializer.Deserialize<BuyerDto>(buyerJson, options);
            Console.WriteLine($"Deserialized Merchant: email={buyer?.email}, Name={buyer?.name}");
            return buyer;
        }

        Console.WriteLine($"Failed to fetch merchant with status code: {response.StatusCode}");
        return null;
    }
}
