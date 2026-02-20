using Shared;
using Shared.DTOs.NewFolder.ProductModule;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Contracts
{
    public interface IProductService
    {
       
        // GetAllProduct
        Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters parameters);
        
        // GetAllBrands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        
        // GetAllTypes
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync(); 
        
        // GerProductById
        Task<ProductResultDto> GetProductByIdAsync(int id);
    }
}
