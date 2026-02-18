using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public String ErrorMessage { get; set; } = string.Empty;
    }
}
