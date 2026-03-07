using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.Contracts;
using Shared.DTOs.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class PaymentsController : ApiController
    {
        private readonly IServiceManager _serviceManager;

        public PaymentsController( IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntentId( string basketId)
           => Ok( await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId));

      
        
        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];
           
            await _serviceManager.PaymentService.UpdatePaymentStatusAsync(json, signatureHeader);
            return new EmptyResult();
        }

    }
}
