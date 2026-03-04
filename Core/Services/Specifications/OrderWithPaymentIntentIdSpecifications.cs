using Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class OrderWithPaymentIntentIdSpecifications : BaseSpecifications<Order , Guid>
    {
        public OrderWithPaymentIntentIdSpecifications( string paymentIntentId) 
              : base(o => o.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
