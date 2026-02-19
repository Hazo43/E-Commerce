using Domain.Contracs;
using Domain.Entities.BasketModule;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Presistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer _connection)
        {
            _database = _connection.GetDatabase();
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);

            var IsCreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonBasket, timeToLive ?? TimeSpan.FromDays(30));

            if (IsCreatedOrUpdated == true)
               return await GetBasketByIdAsync(basket.Id); 
            else
                return null;
            
        }

        public Task<bool> DeleteBasketAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerBasket?> GetBasketByIdAsync(string id)
        {
            var result = await _database.StringGetAsync(id);
            if(result.IsNullOrEmpty)
                return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(result!);
        }
    }
}
