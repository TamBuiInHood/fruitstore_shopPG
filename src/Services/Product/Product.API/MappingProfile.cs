using AutoMapper;
using Infrastructure.Mapping;
using Product.API.Entities;
using Shared.DTOs;

namespace Product.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogProduct, ProductDto>();
            CreateMap<CreateProductDto, ProductDto>();
            CreateMap<UpdateProductDto , ProductDto>().IgnoreAllNonExisting();

        }
    }
}
