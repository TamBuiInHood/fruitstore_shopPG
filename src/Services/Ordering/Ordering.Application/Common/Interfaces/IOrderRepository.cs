using Contracts.Common.Interfaces;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderRepository : IRepositoryBaseAsync<Order, long>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string UserName);

        Task<Order> CreateOrder(Order order);
        Task<Order> UpdateOrder(Order order);
    }
}
