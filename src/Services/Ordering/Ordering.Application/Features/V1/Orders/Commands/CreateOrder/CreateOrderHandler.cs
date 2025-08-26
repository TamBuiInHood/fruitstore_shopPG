using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ILogger _logger;

        public CreateOrderHandler(IMapper mapper, IOrderRepository repository, ILogger logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        private static string MethodName = "CreateOrderHandler";
        public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - {request.UserName}");
            var order = _mapper.Map<Order>(request);
            _repository.Create(order);
            // add su kien event sourcing len tren domain
            order.AddedOrder();
            await _repository.SaveChangesAsync();
            _logger.Information($"END: {MethodName} - {request.UserName}");
            return new ApiSuccessResult<long>(order.id);
        }
    }
}
