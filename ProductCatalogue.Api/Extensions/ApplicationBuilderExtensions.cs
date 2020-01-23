using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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

        public static void UseCustomExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    // log the exception
                    var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var routeWhereExceptionOccurred = exceptionFeature.Path;
                    var exceptionThatOccurred = exceptionFeature.Error;
                    logger.LogError(exceptionThatOccurred, "unhandled exception");

                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An expected fault happened. Try again later.");
                });
            });
        }
    }
}
