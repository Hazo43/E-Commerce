using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ProductModule
{
    public record TypeResultDto
    {
        // Get All Types Return IEnumerable Of Types Data Which Will be
        // {Id , Name}
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
