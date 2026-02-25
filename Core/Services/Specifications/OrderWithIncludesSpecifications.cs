using Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class OrderWithIncludesSpecifications : BaseSpecifications<Order , Guid>
    {

        //  id عشان نعمل فلتر ب ال Where اللي هيه ال Criteria دي هنستخدم فيها ال
        //  Order معاه و هو راجع ب ال (DeliveryMethod , OrderItems) عشان عاوزين نجيب ال Includes و بردو عاوزين نستخدم ال
        // Get Order By Id ==> Criteria ==> id == o.id ==> Includes (DeliveryMethod , OrderItems)
        public OrderWithIncludesSpecifications( Guid id) : base( o => o.Id == id ) 
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
        }

        // Email عشان هنعمل فلتر ب ال Criteria دي بردو هتبقي ليها 
        // (DeliveryMethod , OrderItems) اللي معانا هتبقي هيه هيه Includes و ال
        // Get All OrderByEmail ==> Criteria ==> Email == o.Email ==> Includes (DeliveryMethod , OrderItems)

        public OrderWithIncludesSpecifications(string userEmail) : base( o => o.UserEmail == userEmail ) 
        {

            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
            AddOrderBy(o => o.OrderDate);  // هيه مش مهمه بس عملناها عشان الترتيب
        }
    }
}
