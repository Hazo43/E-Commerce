using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record ProductResultDto
    {
        // 1- Get All Products Return IEnumerable Of Products Data Which Will be
        // {Id , Name, Description , PictureUrl , Price , ProductBrand, ProductType} 

        // 2- Get Product By Id Return Product Data Which Will be
        // {Id , Name, Description , PictureUrl , Price , ProductBrand, ProductType} 

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
    }
}
