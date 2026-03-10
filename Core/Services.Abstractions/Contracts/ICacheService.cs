using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Contracts
{
    public interface ICacheService
    {
        // Get ==> Cached الي معمولها value بترجع ال
        Task<string?> GetCachedValueAsync(string key);
        // Set
        Task SetCacheValueAsync(string key, object value, TimeSpan duration);
    }
}
