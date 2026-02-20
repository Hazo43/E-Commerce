using AutoMapper;
using Domain.Entities.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.NewFolder.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    internal class PictureUrlResolver : IValueResolver<Product, ProductResultDto, string>
    {
        private readonly IConfiguration _configuration;
        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
        {
            
            // فاضيه string  رجعلو Empty او Null لو الصوره ب
            if (string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;

            // وبعدين نحط وراها امتداد الصوره BaseUrl و هتجيب من عندها ال URLs ال Section هتخش جوا  _configuration ال
            return $"{_configuration.GetSection("URLs")["BaseUrl"]}{source.PictureUrl}";
           
         
        }
    }
}
