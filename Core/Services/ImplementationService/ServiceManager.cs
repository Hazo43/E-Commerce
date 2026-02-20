using AutoMapper;
using Domain.Contracs;
using Services.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImplementationService
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper , IBasketRepository _basketRepository) : IServiceManager
    {

        private readonly Lazy<IProductService> _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork , _mapper));
        private readonly Lazy<IBasketService> _basketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));
       
        public IProductService ProductService =>_productService.Value;
        public IBasketService BasketService =>_basketService.Value;
    }
}
