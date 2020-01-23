using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogue.Api.ViewModels.Responses;
using System.Linq;

namespace ProductCatalogue.Api.Extensions
{
    public static partial class MvcBuilderExtensions
    {
        public static void AddApiBehaviorOptions(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

                    var errorResponse = new ErrorResponse();

                    foreach (var error in errorsInModelState)
                    {
                        foreach (var subError in error.Value)
                        {
                            var errorModel = new ErrorModel
                            {
                                FieldName = error.Key,
                                Message = subError
                            };

                            errorResponse.Errors.Add(errorModel);
                        }
                    }

                    return new UnprocessableEntityObjectResult(errorResponse)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });
        }
    }
}
