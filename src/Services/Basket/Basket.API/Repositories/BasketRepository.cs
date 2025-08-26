using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCacheService;
        private readonly ISerializeServices _serializeServices;
        private readonly ILogger _logger;
        public BasketRepository(IDistributedCache redisCacheService, ILogger logger, ISerializeServices serializeServices)
        {
            _redisCacheService = redisCacheService;
            _logger = logger;
            _serializeServices = serializeServices;
        }

        public async Task<Cart?> GetBaskeyByUserName(string UserName)
        {
            _logger.Information($"BEGIN: GetBasketByUserName {UserName}");
            var basket = await _redisCacheService.GetStringAsync(UserName);
            _logger.Information($"END: GetBasketByUserName {UserName}");
            return String.IsNullOrEmpty(basket) ? null : _serializeServices.Deserialize<Cart>(basket);
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

    }
}
