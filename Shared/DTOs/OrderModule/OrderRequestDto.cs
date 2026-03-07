using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderModule
{
    public record OrderRequestDto
    {
        public string BasketId { get; init; } = string.Empty;
        public ShippingAddressDto ShipToAddress  { get; init; }
        public int DeliveryMethodId { get; init; }
    }
}
