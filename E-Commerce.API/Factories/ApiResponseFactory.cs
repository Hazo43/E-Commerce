using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;



namespace E_Commerce.API.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            //Context ==> errors , Key [Faild]
            // context.modelstate ==> <string , modelstateEntry>
            // string ==> name of the faild   اللي حصل فيه الايرور faild دي ال
            // modelstateEntry ==> Errors ==> Error Message 

            //  ValidationError في ال Select هروح اعملو  error لو فيه اي  ModelState في ال errors بتفحص بيشوف فيه اي
            var errors = context.ModelState
                .Where(error => error.Value?.Errors.Any() == true).Select(error => new ValidationError()
                {
                    Field = error.Key,
                    Errors = error.Value.Errors.Select(error => error.ErrorMessage)
                });

            // ValidationError دا شكل الداتا اللي هترجع لو حصل اي
            var response = new ValidationErrorResponse()
            {
                Errors = errors,
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = "One Or More Validation Error happened",
            };
            
            return new BadRequestObjectResult(response);

        }
    }
}
