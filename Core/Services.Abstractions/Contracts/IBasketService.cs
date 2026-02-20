using Shared.DTOs.NewFolder.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Contracts
{
    public interface IBasketService
    {
        // Get 
        Task<BasketDto> GetBasketAsync(string id);

        // Delete
        Task<bool> DeleteBasketAsync(string id);

        // Create Or Update
        Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDTO);
    }
}
