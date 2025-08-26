using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
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

namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IOrderRepository _repository;

        public UpdateOrderHandler(IMapper mapper, ILogger logger, IOrderRepository repository)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        private static string MethodName = "UpdateOrder";
        public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Id: {request.Id}");
            var order = await _repository.GetByIdAsync(request.Id);
            if (order is null) throw new NotFoundException(nameof(order), request.Id);
            
            _mapper.Map(request, order);
            await _repository.UpdateSaveAsync(order);
            await _repository.SaveChangesAsync();
            var result = _mapper.Map<OrderDto>(order);
            // Sendmail
            _logger.Information($"END: {MethodName} - Id: {request.Id}");
            return new ApiSuccessResult<OrderDto>(result);
        }

    }
}
