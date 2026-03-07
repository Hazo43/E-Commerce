using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DeliveryMethodIdNotFountExceptions : NotFoundException
    {
        public DeliveryMethodIdNotFountExceptions(string deliverymethodid) 
                : base($"Basket With Id {deliverymethodid} Not Found")
        {
            
        }
    }
}
