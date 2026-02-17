using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductSpecificationsParameters
    {
        public int? typeId { get; set; }
        public int? brandId { get; set; }
        public ProductSortingOptions sort {  get; set; }

    }
}
