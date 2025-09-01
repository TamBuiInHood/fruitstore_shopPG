using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<Cart?> GetBaskeyByUserName(string UserName);
        Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null!);
        Task<bool> DeleteBaskeyFromUserName(string UserName);
        Task<string> CheckStockQuantityEnough(Cart cart);
    }
}
