using AutoMapper;
using Domain.Entities.BasketModule;
using Shared.DTOs.BasketModule;
using Shared.DTOs.NewFolder.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket , BasketDto>().ReverseMap();
            CreateMap<BasketItem , BasketItemDto>().ReverseMap();
        }
    }
}
