using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class OrderNotFoundExceptions : NotFoundException
    {
        public OrderNotFoundExceptions( Guid id) : base($" Order With Id {id} Not Found")
        {
            
        }
    }
}
