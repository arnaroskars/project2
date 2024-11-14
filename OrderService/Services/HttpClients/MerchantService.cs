using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OrderService.Services.HttpClients.Interfaces;
using OrderService.Models;

public class MerchantServiceClient : IMerchantServiceClient
{
    private readonly HttpClient _httpClient;

    public MerchantServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MerchantDto?> GetMerchantByIdAsync(int merchantId)
    {
        var response = await _httpClient.GetAsync($"api/merchants/{merchantId}");
        if (response.IsSuccessStatusCode)
        {
            var merchantJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Received JSON from MerchantService: {merchantJson}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var merchant = JsonSerializer.Deserialize<MerchantDto>(merchantJson, options);
            Console.WriteLine($"Deserialized Merchant: email={merchant?.email}, Name={merchant?.name}");
            return merchant;
        }

        Console.WriteLine($"Failed to fetch merchant with status code: {response.StatusCode}");
        return null;
    }
}
