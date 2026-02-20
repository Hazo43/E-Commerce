using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    // عشان محدش يعرف يورث منو sealed دي
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(int id)
            : base($"Product With Id {id} Not Found")
        {
            
        }
    }
}
