using AutoMapper;
using Domain.Entities.ProductModule;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Types
            CreateMap<ProductType, TypeResultDto>();

            // Brand
            CreateMap<ProductBrand, BrandResultDto>();

            // Product ( {Get All} , {Get By Id} )
            CreateMap<Product, ProductResultDto>()
                 .ForMember(dest => dest.BrandName, options => options.MapFrom(Scr => Scr.ProductBrand.Name))
                 .ForMember(dest => dest.TypeName, options => options.MapFrom(Scr => Scr.productType.Name));


        }
    }
}
