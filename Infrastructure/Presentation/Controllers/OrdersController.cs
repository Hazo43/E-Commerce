using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.Contracts;
using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private readonly IServiceManager _serviceManager;
        public OrdersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        // CreateOrder
        [HttpPost]
        public async Task<ActionResult<OrderResultDto>> CreateOrderAsync(OrderRequestDto orderRequest)
        {
            // Email بس انا هنا مش هبعت ال Email و  OrderRequestDto كانت المفروض بتاخد CreateOrderAsync دي  Method ال
            // Token نفسو من ال Email غير لما يمون مسجل عندي ف انا هجيب ال CreateOrder يعني محدش هيخش يعمل  [Authorize] هخليه Controller انا هعمل ال        

            // اللي عندي User  بتاع ال data من ال Email كدا انا جبت ال
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _serviceManager.OrderService.CreateOrderAsync(orderRequest, userEmail);
            return Ok(orders);

        }

        // GetOrderById
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResultDto>> GetOrderByIdAsync(Guid id)
        {
            var orderId = await _serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(orderId);
        }

        // GetOrderByemail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResultDto>>> GetAllOrdersByEmail()
        {
            // زي ما عملت فوق Token لا انا هروح اجيبو من GetAllOrdersByEmail دي method في ال Email بدل ما ابعت ال

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _serviceManager.OrderService.GetAllOrderByEmailAsync(userEmail);
            return Ok(order);
        }

        // GetDeliveryMethods
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethodsAsync()
        {
           var delivery = await _serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(delivery);
        }
    }
}
