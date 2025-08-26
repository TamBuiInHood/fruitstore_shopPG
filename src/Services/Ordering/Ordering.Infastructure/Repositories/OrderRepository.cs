using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order, long, OrderContext>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext, IUnitOfWork<OrderContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orders = await FindByCondition(x => x.UserName.Equals(userName)).ToListAsync();
            return orders;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            await CreateSaveAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            await UpdateSaveAsync(order);
            return order;
        }
    }
}
