using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record BrandResultDto
    {
        // Get All Brands Return IEnumerable Of Brands Data Which Will be
        // {Id , Name}

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
