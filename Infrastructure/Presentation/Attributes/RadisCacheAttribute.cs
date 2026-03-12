using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    internal class RadisCacheAttribute(int durationInSeconds  = 120) : ActionFilterAttribute
    {
        //1]  Response و هظبط من خلالو ال Request اللي هلاقي جواها ال ActionExecutingContext دا
        //     --> (context) HttpContext جواها ال

        //2]   بقدر من خلالو اعمل حاجتين next دا
        // -->(1) caching اللي واقف عليها ودي هتحصل لو مش عامل EndPoint ف هيخش جوا ال Invoke اللي واقف عليه يعني هقولوEndPoint ممكن اروح انادي ع ال
        // -->(2) اللي بعديه ActionFilter ممكن يخش او ينادي علي ال
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //1] نركز وصلت ليها ازاي ها cacheService عاوزين نجيب ال
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;
            //2] Add هروح اعملها caching هرجعها لو مش معمولها caching هشوف لو الداتا معمولها 
            //=> بتاعها Key ولا لا لازم اوصل ل ال caching عشان اعرف الداتا دي معمولها
            // Key ==> path Url + Query string 
            // Path Url ==> context.HttpContext.Request.Path     -->  // api/Product
            // Query string ==> context.HttpContext.Request.Query --> // Key , Value مكون من
            //    api/Product?sort=NameDesc ==>  value هيه ال NameDesc و ال Key هو ال Sort ال
            
            string Key = GenerateKey(context.HttpContext.Request);

            //3] نشوف الداتا موجوده ولا لا Radis هنروح نكلم ال
            // ولا لا GetCachedValueAsync موجود قبل كدا في ال Key بشوف ال
            var result = await cacheService.GetCachedValueAsync(Key);
            if (result != null) // قبل كدا caching كدا معناها ان الداتا معمولها
            {                  // Response ف انا المسئول ان اظبط الداتا و ارجعها في ال

                context.Result = new ContentResult
                {
                    Content = result,   // اللي جات من الداتا بيز result هرجعالو ال
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            // caching يعني الداتا بتعتي مش معول ليها Endpoint لو اول مرا اكلم ال
            var resultContext = await next.Invoke(); // Endpoint كدا هو دخل ع ال

            // عندي Caching هروح بقا اعملها Data Base بتاعتي و راح جاب الداتا من ال Endpoint بعد اما دخل ع ال
            //  200OK يكون result الاول ان لازم ال Check هروح اعمل 
           
            if(resultContext.Result is OkObjectResult okObjResult) // okObjResult لو هو صح يعن خزنو هنا
            {
                // مهم
                // value ال okObjResult دي
                //  دقيقتين default قد اي و لو مبعتش انا حاطيت ال cache يبعت معاها المده هيه هتتعملها RadisCacheعشان اللي هيستخدم ال class انا معرفه فوق ف ال durationInSeconds دي 
                await cacheService.SetCacheValueAsync(Key, okObjResult, TimeSpan.FromSeconds(durationInSeconds));
            }
           
        
        }
        private string GenerateKey(HttpRequest request)
        {
            //1] String Variable ==> Add Path /api/Product
            //          Variable ==> Add Query String Values 
            var key = new StringBuilder();
            key.Append(request.Path); // api/products
            
            foreach (var item in request.Query.OrderBy( x => x.Key)) // Sort  الي هيه مثلا هنا ال Key رتب حسب ال
            {
                //  api/Product?sort-NameDesc  <-- كدا cache المفروض هيتخزن عندي ف ال Url ال 
                key.Append($"{item.Key}-{item.Value}");
            }
            // عندي Value بعد ما خزنتو هو و ال key هرجعلو ال
            return key.ToString();

        }
    }
}
