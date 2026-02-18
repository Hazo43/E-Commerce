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
            }
            catch(Exception ex)  
            {
                _logger.LogError($" Something Went Wrong ==> : {ex.Message}");
                await HandleExceptionAsync(context , ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //1] change StatusCode
            context.Response.StatusCode = StatusCodes.Status500InternalServerError; 
           
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
