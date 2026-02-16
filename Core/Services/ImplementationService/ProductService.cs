using AutoMapper;
using Domain.Contracs;
using Domain.Entities.ProductModule;
using Services.Abstractions.Contracts;
using Services.Specifications;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImplementationService
{
    public class ProductService : IProductService
    {
        // 1- object from unitofwork => generic repository عشان نتكلم مع ال  => GetAllBrand()  هترجعلي كل ال  => IEnumerable<ProductBrand>
        // 2- Mapping -> From [IEnumerable<ProductBrand>] ==> IEnumerable<BrandResultDto>  =>  AutoMapper من خلال ال

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand , int>().GetAllAsync();
             // BrandDTO الي  ProductBrand  هحول من
            var brandResult = _mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return brandResult;
        }

        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
        {
            var speification = new ProductWithTypeAndBrandSpecifications();
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(speification);
            var productResult = _mapper.Map<IEnumerable<ProductResultDto>>(products);
            return productResult;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
           var types = await _unitOfWork.GetRepository<ProductType , int>().GetAllAsync();
            var typeResult = _mapper.Map<IEnumerable<TypeResultDto>>(types);
            return typeResult;
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var specification = new ProductWithTypeAndBrandSpecifications(id);
            var product = await _unitOfWork.GetRepository<Product , int>().GetByIdAsync(specification);
            var productResult = _mapper.Map<ProductResultDto>(product);
            return productResult;
        }
    }
}
