using AutoMapper;
using Infrastructure.Mapping;
using Shared.DTOs.CustomerDTO;

namespace Customer.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Entities.Customer>();
            CreateMap<UpdateCustomerDto, Entities.Customer>().IgnoreAllNonExisting();
        }
    }
}
