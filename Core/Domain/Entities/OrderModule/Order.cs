using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = string.Empty;
        public ShippingAddress ShippingAddress { get; set; } 
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public OrderPaymentStatus OrderPaymentStatus { get; set; } = OrderPaymentStatus.Pending;
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int? DeliverMethodId { get; set; }
        public decimal SubTotal { get; set; } // Total Price Of Item 
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public string PaymentIntentId { get; set; } = string.Empty;
     
       
    }
}
