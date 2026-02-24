using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderModule
{
    public record OrderItemDto
    {
        public decimal Price { get; init; }
        public int Quantity { get; init; }
        public int ProductId { get; init; }
        public string ProductName { get; init; } = default!;
        public string PictureUrl { get; init; } = default!;
    }
}
