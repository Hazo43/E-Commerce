using Domain.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product , int>
    {
        // Get All Product => Include (ProductType , ProductBrand)
        public ProductWithTypeAndBrandSpecifications() : base(null)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.productType);
        }

        // Get Product By Id (int id ) => Include Type , Brand [Include] . Where [Crietria]
        public ProductWithTypeAndBrandSpecifications(int id) : base( p=> p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.productType);
        }
    }
}
