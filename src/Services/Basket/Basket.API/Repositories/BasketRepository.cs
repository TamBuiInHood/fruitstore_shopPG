using Basket.API.Entities;
using Basket.API.Extensions;
using Basket.API.Extensions.Service.Interface;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Contracts.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Shared.DTOs.InventoryDTO;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCacheService;
        private readonly ISerializeServices _serializeServices;
        private readonly ILogger _logger;
        private readonly IInventoryServiceClient _client;

        public BasketRepository(IDistributedCache redisCacheService, ILogger logger, ISerializeServices serializeServices, IInventoryServiceClient httpClient)
        {
            _redisCacheService = redisCacheService;
            _logger = logger;
            _serializeServices = serializeServices;
            _client = httpClient;
        }

        public async Task<Cart?> GetBaskeyByUserName(string UserName)
        {
            _logger.Information($"BEGIN: GetBasketByUserName {UserName}");
            var basket = await _redisCacheService.GetStringAsync(UserName);
            _logger.Information($"END: GetBasketByUserName {UserName}");

            if (string.IsNullOrEmpty(basket)) return null;

            var cart = _serializeServices.Deserialize<Cart>(basket);

            foreach (var item in cart.Items)
            {
                var inventory = await _client.GetQuantityThrougtApi(item.ItemNo);
                item.AvailableQuantity = inventory;
            }
            return string.IsNullOrEmpty(basket) ? null : cart;
        }

        public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
        {
            _logger.Information($"BEGIN: UpdateBasket for {cart.Username}");
            if (options != null)
            {
                await _redisCacheService.SetStringAsync(cart.Username, _serializeServices.Serialize(cart), options);
            }
            else
            {
                await _redisCacheService.SetStringAsync(cart.Username, _serializeServices.Serialize(cart));
            }

            foreach (var item in cart.Items)
            {
                var inventory = await _client.GetQuantityThrougtApi(item.ItemNo);
                item.AvailableQuantity = inventory;
            }
            _logger.Information($"END: UpdateBasket for{cart.Username}");
            return await GetBaskeyByUserName(cart.Username);
        }

        public async Task<bool> DeleteBaskeyFromUserName(string UserName)
        {
            try
            {
                _logger.Information($"BEGIN: DeleteBasket for {UserName}");
                await _redisCacheService.RemoveAsync(UserName);
                _logger.Information($"END: DeleteBasket for {UserName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error DeleteBaskeyFromUserName" + ex.Message);
                throw;
            }
        }

        public async Task<string> CheckStockQuantityEnough(Cart cart)
        {
            _logger.Information($"BEGIN: CheckStockQuantityEnough of {cart.Username}");
            foreach (var item in cart.Items)
            {
                var inventory = await _client.GetQuantityThrougtApi(item.ItemNo);
                if (item.Quantity > inventory) return $"Item no {item.ItemNo} is not enought." ;
            }
            _logger.Information($"END: CheckStockQuantityEnough of {cart.Username}");
            return null;
        }
    }
}
