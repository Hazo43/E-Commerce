using Shared.DTOs;
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
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? typeId , int? brandId);
        
        // GetAllBrands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        
        // GetAllTypes
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync(); 
        
        // GerProductById
        Task<ProductResultDto> GetProductByIdAsync(int id);
    }
}
