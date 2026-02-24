using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderModule
{
    public class OrderItem : BaseEntity<Guid>
    {
        public OrderItem()
        {
            
        }
        public OrderItem(decimal price, int quantity, ProductInOrderItem product, Order order, int orderId)
        {
            Price = price;
            Quantity = quantity;
            Product = product;
        }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductInOrderItem Product { get; set; } 

    }
}
