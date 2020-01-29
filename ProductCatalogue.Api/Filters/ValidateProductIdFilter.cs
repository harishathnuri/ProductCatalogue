using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ProductCatalogue.Api.Services;
using System;

namespace ProductCatalogue.Api.Filters
{
    public class ValidateProductIdFilter : ActionFilterAttribute
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ValidateProductIdFilter> _logger;

        public ValidateProductIdFilter(IProductRepository repository, ILogger<ValidateProductIdFilter> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogDebug("Start - ValidateProductIdFilter - OnActionExecuting");

            var isProductIdAvailable = context.ActionArguments.TryGetValue("productId", out object foreignKey);
            var productId = Guid.Parse(foreignKey.ToString());

            if (!_repository.ProductExists(productId))
                context.Result = new NotFoundObjectResult($"Invalid product id {productId}");

            _logger.LogDebug("End - ValidateProductIdFilter - OnActionExecuting");
        }
    }
}
