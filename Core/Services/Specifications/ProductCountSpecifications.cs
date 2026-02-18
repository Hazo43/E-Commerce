using Domain.Entities.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductCountSpecifications : BaseSpecifications<Product , int>
    {
        public ProductCountSpecifications(ProductSpecificationsParameters parameters )
             : base(p => (!parameters.typeId.HasValue || p.TypeId == parameters.typeId) &&  // type
                         (!parameters.brandId.HasValue || p.BrandId == parameters.brandId) && // brand
                         (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower())))  // Search
        {
            
        }
    }
}
