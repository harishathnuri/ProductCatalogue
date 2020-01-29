using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductCatalogue.Api.ViewModels.Responses;
using System;
using System.Net;

namespace ProductCatalogue.Api.Filters
{
    public class ExceptionHandlerFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionHandlerFilter> _logger;

        public ExceptionHandlerFilter(ILogger<ExceptionHandlerFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogDebug("Start - ExceptionHandlerFilter - OnException");

            base.OnException(context);

            var fault = new FaultModel
            {
                Id = Guid.NewGuid().ToString(),
                Status = (short)HttpStatusCode.InternalServerError,
                Title = "Some kind of error occured in the API. Please use the id and contact out " +
                    "support team if the problem persits."
            };

            var innerExMessage = GetInnerMostExceptionMessage(context.Exception);

            _logger.LogError(context.Exception, innerExMessage + $" -- {fault.Id}");

            context.ExceptionHandled = true;
            var result = JsonConvert.SerializeObject(fault);
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.HttpContext.Response.WriteAsync(result);

            _logger.LogDebug("End - ExceptionHandlerFilter - OnException");
        }

        private string GetInnerMostExceptionMessage(Exception exception)
        {
            if (exception.InnerException != null)
                return GetInnerMostExceptionMessage(exception.InnerException);

            return exception.Message;
        }
    }
}
