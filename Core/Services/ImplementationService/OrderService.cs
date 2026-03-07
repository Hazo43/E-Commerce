using AutoMapper;
using Domain.Contracs;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.SignalR;
using Services.Abstractions.Contracts;
using Services.Specifications;
using Shared.DTOs.OrderModule;
using System;
using System.Collections;
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
        // Create Order
        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail)
        {
            // 1] Map From ShippingAddressDto To ShippingAdress
            var shippingAddress = _mapper.Map<ShippingAddressDto, ShippingAddress>(orderRequest.ShipToAddress);
           
            // 2] GetOrderItem ==> Id ومنها هجيب ال BaskeItem ومنها هجيب ال Basket عن طريقو هروح اجيب ال request اللي جوا ال BasketId ف محتاج اوصل ل
            var basket = await _basketRepository.GetBasketAsync(orderRequest.BasketId);
            if (basket is null)
                throw new BasketNotFoundException(orderRequest.BasketId);
            //
            List<OrderItem> orderitems = new List<OrderItem>();

            foreach (var item in basket.Items)
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
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
           
            // 3] GetDeliveryMethod ==>  DeliveryMethodId دي عشان اجيبها محتاج اخد ال
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                .GetByIdAsync(orderRequest.DeliveryMethodId);
            if(deliveryMethod is null)
                throw new DeliveryMethodNotFountExceptions(orderRequest.DeliveryMethodId);

            // 4] Calculate SubTotal 
            // الكميه Quantity في ال Product هيضرب السعر بتاع كل orderitems كلهم من خلال ال Product بتاع ال subTotal دا هيحسب ال
            var subTotal = orderitems.Sum( o => o.Price * o.Quantity);

            // دا مةجود ولا لا paymentIntentId اللي ب ال order عاوز افحص اشوف ال
            var orderExist = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId));
          
            //  يبقي هو موجود قبل كدا ف هروح امسحوorder لو رجعت orderExist دي
            if (orderExist != null)
            {
                // معاه orderItems يمسح ال Order عشان لما يمسح ال OrderItem من عند ال cascade اعملها orderConfiguration هروح بردو علي ال
                orderRepo.Delete(orderExist);
            }
           

            // 5] Create Oblect From Order ==> Parameters هبعتلو ال
            var OrderToCreate = new Order(userEmail, shippingAddress, orderitems, deliveryMethod, subTotal , basket.PaymentIntentId);
           
            // DataBase في ال OrderToCreate هروح اضيف ال
            await orderRepo.AddAsync(OrderToCreate);
            await _unitOfWork.SaveChangesAsync();

            // 6] Map<Order , OrderResult>();
           return  _mapper.Map<Order, OrderResultDto>(OrderToCreate);
        }

        // Get All Orders By Email
        public async Task<IEnumerable<OrderResultDto>> GetAllOrderByEmailAsync(string userEmail)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>()
                 .GetAllAsync(new OrderWithIncludesSpecifications(userEmail));

            // OrderResultDto و هو عاوز اللي يرجع Order عباره عن  (order) عشان اللي راجع هنا map انا هعمل 
            return _mapper.Map <IEnumerable<Order>, IEnumerable<OrderResultDto>>(orders);

        }

        // Get Delivery Method 
        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            // Map ف عشان كدا عملنا  IEnumerable<DeliveryMethodDto اللي هو عاوزو هو عاوز return مش دا ال  (deliveryMethod) اللي راجع هنا
            return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(deliveryMethod);
        }

        // Get Order By Id
        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>()
                   .GetByIdAsync(new OrderWithIncludesSpecifications(id)) ?? throw new OrderNotFoundExceptions(id);

            // OrderResultDto و هو عاوز اللي يرجع Order عباره عن  (order) عشان اللي راجع هنا map انا هعمل 
            return _mapper.Map<Order , OrderResultDto>(orders);
        
        }
    }
}
