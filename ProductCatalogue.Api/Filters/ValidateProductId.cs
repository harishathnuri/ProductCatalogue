using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ProductCatalogue.Api.Services;
using System;

namespace ProductCatalogue.Api.Filters
{
    public class ValidateProductId : ActionFilterAttribute
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ValidateProductId> _logger;

        public ValidateProductId(IProductRepository repository, ILogger<ValidateProductId> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogDebug("Start - ValidateProductId - OnActionExecuting");

            var isProductIdAvailable = context.ActionArguments.TryGetValue("productId", out object foreignKey);
            var productId = Guid.Parse(foreignKey.ToString());

            if (!_repository.ProductExists(productId))
                context.Result = new NotFoundObjectResult($"Invalid product id {productId}");

            _logger.LogDebug("End - ValidateProductId - OnActionExecuting");
        }
    }
}
