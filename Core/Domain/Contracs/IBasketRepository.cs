using Domain.Entities.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracs
{
    public interface IBasketRepository
    {
        // Get Basket By Id
        Task<CustomerBasket?> GetBasketByIdAsync(string  id);

        // Create Or Update Basket
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null);
        
        // Delete Basket
        Task<bool> DeleteBasketAsync(string id);
    }
}
