using Domain.Contracs;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistence.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer _connection)
        {
            _database = _connection.GetDatabase();
        }

        public async Task<string?> GetAsync(string key)
        {
            // Radis Value  من نوع  value و بترجع  caching لو معملو key بتجيب ال StringGetAsync دي  
            var value = await _database.StringGetAsync(key);
            // value  لو هيه م فاضيه هرجعلو ال default رجعلو ال IsNullOrEmpty لو هيه
            return value.IsNullOrEmpty ? default : value;
        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            // Json الي C# من  value هحول ال
            var SerializedOje = JsonSerializer.Serialize(value);
            // [ Radis Key -> (key) , Radis Value -> (SerializedOje) , duration  ]  ثلاثه اشياء StringSetAsync دي 
            await _database.StringSetAsync(key, SerializedOje, duration);
            throw new NotImplementedException();
        }
    }
}
