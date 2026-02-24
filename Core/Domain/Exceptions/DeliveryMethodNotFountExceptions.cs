using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DeliveryMethodNotFountExceptions : NotFoundException
    {
        public DeliveryMethodNotFountExceptions(int id) 
            : base($"DeliveryMethod With Id : {id} Not Found")
        {
            
        }
    }
}
