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
        public ProductWithTypeAndBrandSpecifications(int? typeId, int? brandId)
            : base( p => (!typeId.HasValue || p.TypeId == typeId) && 
                         (!brandId.HasValue || p.BrandId == brandId))
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
