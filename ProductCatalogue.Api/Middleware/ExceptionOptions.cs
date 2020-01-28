using Microsoft.AspNetCore.Http;
using ProductCatalogue.Api.ViewModels.Responses;
using System;

namespace ProductCatalogue.Api.Middleware
{
    public class ExceptionOptions
    {
        public Action<HttpContext, Exception, FaultModel> AddResponseDetails { get; set; }
    }
}
