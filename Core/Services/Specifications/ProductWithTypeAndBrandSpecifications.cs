using Domain.Entities.ProductModule;
using Shared;
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
        public ProductWithTypeAndBrandSpecifications(ProductSpecificationsParameters parameters)
            : base( p => (!parameters.typeId.HasValue || p.TypeId == parameters.typeId) &&  // type
                         (!parameters.brandId.HasValue || p.BrandId == parameters.brandId) && // brand
                         ( string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower())))  // Search
        {
            AddInclude(p => p.ProductBrand); 
            AddInclude(p => p.productType);
            // Sort => Switch 4 { NameAsc , NameDes , ProceAsc , PriceDesc ]
            switch(parameters.sort)
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

            // Pagination 
            ApplyPagination(parameters.pageSize, parameters.PageIndex);
        }

        // Get Product By Id (int id ) => Include Type , Brand [Include] . Where [Crietria]
        public ProductWithTypeAndBrandSpecifications(int id) : base( p=> p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.productType);
        }
    }
}
