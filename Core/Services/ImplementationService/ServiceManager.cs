using AutoMapper;
using Domain.Contracs;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Services.Abstractions.Contracts;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImplementationService
{
    public class ServiceManager(IUnitOfWork _unitOfWork,
                               IMapper _mapper,
                               IBasketRepository _basketRepository,
                               UserManager<User> _userManager,
                               IOptions<JwtOptions> _option,
                               IConfiguration _configuration) // :  IServiceManager
    {

        private readonly Lazy<IProductService> _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
        private readonly Lazy<IBasketService> _basketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));
        private readonly Lazy<IAuthenticationService> _authService = new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager, _option, _mapper));
        private readonly Lazy<IOrderService> _orderService = new Lazy<IOrderService>(() => new OrderService(_mapper, _unitOfWork, _basketRepository));
        private readonly Lazy<IPaymentService> _paymentService = new Lazy<IPaymentService>(() => new PaymentService(_configuration, _unitOfWork, _mapper, _basketRepository));
        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService AuthenticationService => _authService.Value;

        public IOrderService OrderService => _orderService.Value;

        public IPaymentService PaymentService => _paymentService.Value;
    }
}
