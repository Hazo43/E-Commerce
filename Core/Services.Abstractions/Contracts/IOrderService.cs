using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Contracts
{
    public interface IOrderService
    {
        // 1 - GetOrderById ==> Take (orderId -> GUID) ==> Return (OrderResult)
        Task<OrderResultDto> GetOrderByIdAsync(Guid id);
        
        // 2 - GetAllOrderByEmail ==> Take (Email) ==> Return ( IEnumerable<OrderResult> )
         Task<IEnumerable<OrderResultDto>> GetAllOrderByEmailAsync(string userEmail);

        // 3 - CreateOrder ==> Take( OrderRequest , Email) => Return ( OrderResult)
        Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail);

        // 4 - GetDeliveryMethods ==> Return (IEnumerable<DeliveryMethodResult>)
        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
    }
}
