using AutoMapper;
using Domain.Entities.OrderModule;
using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Shipping Address
            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();

            // Delivery Method
            CreateMap<DeliveryMethod , DeliveryMethodDto>();

            // OrderItem 
            CreateMap<OrderItem, OrderItemDto>()
                  .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.ProductId))
                  .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                  .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.Product.PictureUrl));

            // Order 
            CreateMap<Order, OrderResultDto>()
                  .ForMember(dest => dest.OrderPaymentStatus, options => options.MapFrom(src => src.OrderPaymentStatus.ToString()))
                  .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod.ShortName))
                  .ForMember(dest => dest.Total, options => options.MapFrom(src => src.SubTotal + src.DeliveryMethod.Price));

        }
    }
}
