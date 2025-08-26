using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;
using Serilog;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, ApiResult<long>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IOrderRepository _repository;

        public DeleteOrderHandler(IMapper mapper, ILogger logger, IOrderRepository repository)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        private static string MethodName = "DeleteOrder";

        public async Task<ApiResult<long>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Id: {request.Id}");
            var order = await _repository.GetByIdAsync(request.Id);
            if (order == null)
                return new ApiResult<long>
                {
                    IsSucceeded = false,
                    Message = $"Delete id {request} fail",

                };
            _repository.Delete(order);
            order.DeletedOrder();
            await _repository.SaveChangesAsync();
            _logger.Information($"END: {MethodName} - Id: {request}");
            return new ApiSuccessResult<long>(request.Id);
        }
    }
}
