namespace Basket.API.Extensions.Service.Interface
{
    public interface IInventoryServiceClient
    {
        Task<double> GetQuantityThrougtApi(string itemNo);
    }
}
