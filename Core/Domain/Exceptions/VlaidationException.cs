using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class VlaidationException : Exception
    {
        public IEnumerable<string> Errors { get; set; } = [];
        public VlaidationException( IEnumerable<string> errors) : base("Validation Faild")
        {
            Errors = errors;
        }
    }
}
