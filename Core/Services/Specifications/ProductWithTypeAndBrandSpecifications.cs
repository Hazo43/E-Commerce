using Domain.Entities.ProductModule;
using Shared.Enums;
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
        public ProductWithTypeAndBrandSpecifications(int? typeId, int? brandId , ProductSortingOptions sort)
            : base( p => (!typeId.HasValue || p.TypeId == typeId) && 
                         (!brandId.HasValue || p.BrandId == brandId))  
        {
            AddInclude(p => p.ProductBrand); 
            AddInclude(p => p.productType);
            // Sort => Switch 4 { NameAsc , NameDes , ProceAsc , PriceDesc ]
            switch(sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending( p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default: AddOrderBy(P => P.Id);
                    break;

            }
        }

        // Get Product By Id (int id ) => Include Type , Brand [Include] . Where [Crietria]
        public ProductWithTypeAndBrandSpecifications(int id) : base( p=> p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.productType);
        }
    }
}
