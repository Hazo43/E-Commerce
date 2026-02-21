using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.BasketModule
{
    public record BasketDto
    {
        // غير قابله للتعديل property معناها بتخلي ال init دي
        public string Id { get; init; } = string.Empty;
        public ICollection<BasketItemDto> BasketItems { get; init; } = [];
    }
}
