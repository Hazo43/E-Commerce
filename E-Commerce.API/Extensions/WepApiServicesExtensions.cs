using E_Commerce.API.Factories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
namespace E_Commerce.API.Extensions
{
    public static class WepApiServicesExtensions
    {
        public static IServiceCollection WepApiServices (this IServiceCollection services , IConfiguration _configuration)
        {
            services.AddControllers();
            // 
            var frontUrl = _configuration.GetSection("URLs")["FrontUrl"];
            services.AddCors(options =>
            {
                //0]  server يعني السماح لموقع مختلف انه يطلب بيانات من ال AddCors دي

                //1] AllowAnyHeader() ==> Frontend بييجي من ال HTTP Header ده معناه إن السيرفر يسمح بأي  
                // مثال Headers: => (Accept , Authorization , Content-Type)

                //2] AllowAnyMethod() ==> HTTP Requests => (Get , Post , Delete , Put) ده معناه السماح بكل أنواع 

                //3]  API بنتحدد الموقع المسموح ليه بس انه يتصل ب ال WithOrigins دي 
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod()
                           .WithOrigins(frontUrl); // http://localhost:4200  Appsetting هفصلو ف ال

                });

            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen( options =>
            {
                // في الواجهه Authorize عشان يظهر زراز ال Bearer اسمه Auth ان فيه نوع  Swagger دا بيعمل اي ؟ بيعرف ال 
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    // 
                    In = ParameterLocation.Header, // Http Header في ال passing بتاعي لازم يحصلها Token معناها ان ال
                    Description = "Please Enter a Valid Token", // Token دي المسدج اللي هتبقي ظاهره وانا بدخل ال
                    Name = "Authorization", // JwtToken اللي انا بستخدمو عشان اباصي ال Http Header بتاع ال Name دا ال
                    Type = SecuritySchemeType.Http, // Http ان انا شغال Swagger بفهم ال
                    BearerFormat = "JWT", // JWT بقولو ان انا شغال بالشكل بتاع ال
                    Scheme = "Bearer"
                });
                // Request يتبعت تلقائي في ال Token ال Authorize يعني لما اضغط APIs يطبق النظام دا علي ال Swagger بيخلي ال
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        // بالتعريف اللي عملناه فوق requirement بيربط ال
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string []{ }
                    }
                });
            });
            // validation Error
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });

            return services;
        }
    }
}
