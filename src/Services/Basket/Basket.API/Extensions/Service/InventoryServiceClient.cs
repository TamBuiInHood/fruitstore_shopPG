using Basket.API.Extensions.Service.Interface;

namespace Basket.API.Extensions.Service
{
    public class InventoryServiceClient : IInventoryServiceClient

    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<InventoryServiceClient> _logger;

        public InventoryServiceClient(HttpClient httpClient, ILogger<InventoryServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<double> GetQuantityThrougtApi(string requestParameter)
        {
            var response = await _httpClient.GetAsync($"api/inventory/items/stock-vailable/{requestParameter}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch inventory for item {ItemNo}", requestParameter);
                return 0;
            }
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);
            return double.Parse(json);
        }
    }
}
