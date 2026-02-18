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
        public string? Search { get; set; }

        // Skip
        private int _pageIndex { get; set; } = 1;
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                //  عادي value  رجع ال ( 1)  لو اكبر من الصفر رجع ال value <= 0 لو 
                _pageIndex = (value <= 0) ? 1 : value;
            }
        }
         
        // Take

        private const int defaultPageSize = 5;
        private const int maxPageSize = 10;

        private int _pageSize = defaultPageSize;
        public int pageSize 
        {
            get { return _pageSize ; }
           
            set 
            {
                if (value <= 0)
                    _pageIndex = defaultPageSize;
                else if (value > maxPageSize)
                    _pageSize = maxPageSize;
                else
                    _pageSize = value;
            }

        }


    }
}
