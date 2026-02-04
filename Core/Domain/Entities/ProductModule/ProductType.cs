using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class ProductType : BaseEntity<int>
    {
        public string Name { get; set; } = default!;

        #region RelathioShip

        // Product (m) -- Type (1)

        public ICollection<Product> Products { get; set; }

        #endregion
    }
}
