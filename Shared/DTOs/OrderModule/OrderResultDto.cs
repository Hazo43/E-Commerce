using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderModule
{
    public record OrderResultDto
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public ShippingAddressDto ShippingAddress { get; init; }
        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
        public string OrderPaymentStatus { get; init; } = string.Empty;
        public string DeliveryMethod { get; init; } = default!;
        public int? DeliverMethodId { get; init; }
        public decimal SubTotal { get; init; } // Total Price Of Item 
        public decimal Total { get; init; } // Total = SubTotal + DeliveryMethod.Price
        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.UtcNow;
        public string PaymentIntentId { get; init; } = string.Empty;
    }
}
