using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderModule
{
    public class DeliveryMethod : BaseEntity<int>
    {
        public DeliveryMethod(string shortName, string description, string deliveryTime, decimal price, ICollection<Order> orders)
        {
            ShortName = shortName;
            Description = description;
            DeliveryTime = deliveryTime;
            Price = price;

        }
        public DeliveryMethod()
        {
            
        }
        public string ShortName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string DeliveryTime { get; set; } = default!;
        public decimal Price { get; set; }
       
        // DeliveryMethod -> 1 , Order -> m

    }
}
