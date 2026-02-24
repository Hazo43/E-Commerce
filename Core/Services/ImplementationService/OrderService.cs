using AutoMapper;
using Domain.Contracs;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstractions.Contracts;
using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImplementationService
{
    internal class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderService( IMapper mapper , IUnitOfWork unitOfWork , IBasketRepository basketRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }
        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail)
        {
            // 1] Map From ShippingAddressDto To ShippingAdress
            var shippingAddress = _mapper.Map<ShippingAddressDto, ShippingAddress>(orderRequest.ShippingAddress);
           
            // 2] GetOrderItem ==> Id ومنها هجيب ال BaskeItem ومنها هجيب ال Basket عن طريقو هروح اجيب ال request اللي جوا ال BasketId ف محتاج اوصل ل
            var basket = await _basketRepository.GetBasketAsync(orderRequest.BasketId);
            if (basket is null)
                throw new BasketNotFoundException(orderRequest.BasketId);
            //
            List<OrderItem> orderitems = new List<OrderItem>();

            foreach (var item in basket.BasketItems)
            {
                // BasketItems عن طريق ال Id ب ال product هنا جبنا ال
                //  عشان نعمل اتشك علي السعر product احنا جبنا ال
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if(product is null)
                    throw new ProductNotFoundException(item.Id);

                // Database اللي جواها ونجيب السعر بتاهم الحقيقي من ال All Products عشان نجيب ال orderItem هنجيب ال
                var orderItem = new OrderItem()
                {
                    // بتاخد خمس حجات انا رحعتهما اللي هما orderItem ال
                    // [ProductId , PictureUrl , ProductName , Price  , Quantity]
                    Product = new ProductInOrderItem { ProductId = product.Id, PictureUrl = product.PictureUrl, ProductName = product.Name },
                    Price = item.Price,
                    Quantity = item.Quantity,
                };
                orderitems.Add(orderItem);
            }
           
            // 3] GetDeliveryMethod ==>  DeliveryMethodId دي عشان اجيبها محتاج اخد ال
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                .GetByIdAsync(orderRequest.DeliveryMethodId);
            if(deliveryMethod is null)
                throw new DeliveryMethodNotFountExceptions(orderRequest.DeliveryMethodId);

            // 4] Calculate SubTotal 
            // الكميه Quantity في ال Product هيضرب السعر بتاع كل orderitems كلهم من خلال ال Product بتاع ال subTotal دا هيحسب ال
            var subTotal = orderitems.Sum( o => o.Price * o.Quantity);

            // 5] Create Oblect From Order ==> Parameters هبعتلو ال
            var OrderToCreate = new Order(userEmail, shippingAddress, orderitems, deliveryMethod, subTotal);
           
            // DataBase في ال OrderToCreate هروح اضيف ال
            await _unitOfWork.GetRepository<Order , Guid>().AddAsync(OrderToCreate);
            await _unitOfWork.SaveChangesAsync();

            // 6] Map<Order , OrderResult>();
           return  _mapper.Map<Order, OrderResultDto>(OrderToCreate);
        }

        public Task<IEnumerable<OrderResultDto>> GetAllOrderByEmailAsync(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderResultDto> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
