using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!; 
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }

        #region RelathionShip

        // Product (m) --  Product Type (1)
        public ProductType productType { get; set; } = default!;
        public int TypeId { get; set; }

        // Product (m) -- Product Brand (1)
        public ProductBrand ProductBrand { get; set; } = default!;
        public int BrandId { get; set; }
        #endregion

    }
}
