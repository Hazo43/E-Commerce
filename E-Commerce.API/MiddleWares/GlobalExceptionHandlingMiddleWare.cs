using Domain.Exceptions;
using Shared.ErrorModels;

namespace E_Commerce.API.MiddleWares
{
    public class GlobalExceptionHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleWare> _logger;

        public GlobalExceptionHandlingMiddleWare(RequestDelegate next , ILogger<GlobalExceptionHandlingMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
                // وهيخش ع اللي بعدها _next  غلط غير كدا هيخش علUrl غير لو if هو مش هيخش هنا في ال
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                    await HandleNotFoundApi(context);
            }
            catch(Exception ex)  
            {
                _logger.LogError($" Something Went Wrong ==> : {ex.Message}");
                await HandleExceptionAsync(context , ex);
            }
        }

        // Error Api Not Found
        private async Task HandleNotFoundApi(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"The EndPoind With Url {context.Request.Path} Not Found" // Request اللي هو بيبعتو في ال Url دي بتجيب ال context.Request.Path
            };
             await context.Response.WriteAsJsonAsync(response);
        }

        // هيخش يتعامل من هنا service اي ايرور ف ال
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //1] change StatusCode
            //context.Response.StatusCode = StatusCodes.Status500InternalServerError; 
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                (_) => StatusCodes.Status500InternalServerError,
            };

            //2] change Content Type
            context.Response.ContentType = "application/json";

            //3] Write Response in body 
            var response = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = ex.Message
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
