using Domain.Contracs;
using Services.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImplementationService
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService( ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }
        public async Task<string?> GetCachedValueAsync(string key)
          => await _cacheRepository.GetAsync(key);

        public async Task SetCacheValueAsync(string key, object value, TimeSpan duration)
           => await _cacheRepository.SetAsync(key, value, duration);
    }
}
