using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracs
{
    public interface ICacheRepository
    {
        // Get ==> in memory عندي ف ال caching هترجع الداتا اللي معمول ليها
       Task<string?> GetAsync(string key);

        // Set ==>  caching و بعدين تجيب منها الداتا وبعدين تتحط في ال EndPoint الاول بتروح تكلم ال caching دي بيكون لسه محصلش 
        Task SetAsync(string key, object value , TimeSpan duration);
    }
}
