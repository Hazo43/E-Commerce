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
    }
}
