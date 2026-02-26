using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class  UserNotFoundExceptions : NotFoundException
    {
        public UserNotFoundExceptions( string email) : 
                                      base($" User With Email : {email} Not Found")
        {
            
        }
    }
}
