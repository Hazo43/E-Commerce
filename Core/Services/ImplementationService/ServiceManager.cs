using AutoMapper;
using Domain.Contracs;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImplementationService
{
    public class ServiceManager(IUnitOfWork _unitOfWork, 
                               IMapper _mapper ,
                               IBasketRepository _basketRepository , 
                               UserManager<User> _userManager) : IServiceManager
    {

        private readonly Lazy<IProductService> _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork , _mapper));
        private readonly Lazy<IBasketService> _basketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));
        private readonly Lazy<IAuthenticationService> _authService = new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager));
        public IProductService ProductService =>_productService.Value;
       
        public IBasketService BasketService =>_basketService.Value;
        
        public IAuthenticationService AuthenticationService => _authService.Value;
    }
}
