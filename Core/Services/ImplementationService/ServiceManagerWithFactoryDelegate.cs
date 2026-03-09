using Services.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImplementationService
{
    public class ServiceManagerWithFactoryDelegate
        (Func<IProductService> _productFactory , 
         Func<IBasketService> _basketFactory , 
         Func<IAuthenticationService> _authenticationFactory , 
         Func<IOrderService> _orderFactory , 
         Func<IPaymentService> _paymentFactory) : IServiceManager
    {
        public IProductService ProductService => _productFactory.Invoke();

        public IBasketService BasketService => _basketFactory.Invoke();

        public IAuthenticationService AuthenticationService => _authenticationFactory.Invoke();

        public IOrderService OrderService => _orderFactory.Invoke();

        public IPaymentService PaymentService => _paymentFactory.Invoke();
    }
}
