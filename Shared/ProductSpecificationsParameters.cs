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
        private const int defaultPageSize = 5;
        private const int maxPageSize = 10;

        public int? typeId { get; set; }
        public int? brandId { get; set; }
        public ProductSortingOptions sort {  get; set; }
        public string? Search { get; set; }

        public int PageIndex { get; set; }

        private int _pageSize = defaultPageSize;

        public int pageSize 
        {
            get { return _pageSize ; }
            // عادي value غير كدا هنحط ال maxPageSize هنحط ال maxPageSize اكبر من ال value لو ال
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }

            // دي صح بردو
            //set
            //{
            //    if (value > maxPageSize)
            //        _pageSize = maxPageSize;
            //    else
            //        _pageSize = value;
            //}

        }


    }
}
