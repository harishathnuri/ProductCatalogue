using Microsoft.AspNetCore.Builder;
using ProductCatalogue.Api.Middleware;
using ProductCatalogue.Api.Options;
using System;

namespace ProductCatalogue.Api.Extensions
{
    public static partial class ApplicationBuilderExtensions
    {
        public static void UseSwaggerForProductCatalogue(
            this IApplicationBuilder app, Action<SwaggerOptions> optionsBuilder)
        {
            var swaggerOptions = new SwaggerOptions();
            optionsBuilder(swaggerOptions);

            app.UseSwagger(options => options.RouteTemplate = swaggerOptions.JsonRoute);
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });
        }

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            var options = new ExceptionOptions();
            return builder.UseMiddleware<ExceptionMiddleware>(options);
        }

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder,
            Action<ExceptionOptions> configureOptions)
        {
            var options = new ExceptionOptions();
            configureOptions(options);

            return builder.UseMiddleware<ExceptionMiddleware>(options);
        }
    }
}
