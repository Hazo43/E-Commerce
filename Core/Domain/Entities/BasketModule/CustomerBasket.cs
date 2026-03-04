using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        public string Id { get; set; } = string.Empty;
        public ICollection<BasketItem> BasketItems { get; set; } = [];
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public decimal? ShippingPrice { get; set; }
        public int? DeliveryMethoId { get; set; }
    }
}
