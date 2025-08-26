using MediatR;
using Ordering.Application.Common.Models;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<ApiResult<long>>
    {
        public long Id { get; private set; }

        public DeleteOrderCommand(long id)
        {
            Id = id;
        }   
    }
}
