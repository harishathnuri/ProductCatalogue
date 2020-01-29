using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductCatalogue.Api.ViewModels.Responses;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ProductCatalogue.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ExceptionOptions _options;
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ExceptionOptions options, RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _options = options;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
           var fault = new FaultModel
            {
                Id = Guid.NewGuid().ToString(),
                Status = (short)HttpStatusCode.InternalServerError,
                Title = "Some kind of error occured in the API. Please use the id and contact out " +
                    "support team if the problem persits."
            };

            _options.AddResponseDetails?.Invoke(context, exception, fault);

            var innerExMessage = GetInnerMostExceptionMessage(exception);

            _logger.LogError(exception, innerExMessage + $" -- {fault.Id}");

            var result = JsonConvert.SerializeObject(fault);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }

        private string GetInnerMostExceptionMessage(Exception exception)
        {
            if (exception.InnerException != null)
                return GetInnerMostExceptionMessage(exception.InnerException);

            return exception.Message;
        }
    }
}
