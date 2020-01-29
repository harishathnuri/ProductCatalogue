using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Filters;
using ProductCatalogue.Api.ViewModels;
using ProductCatalogue.Api.ViewModels.Requests;
using ProductCatalogue.Api.ViewModels.Responses;
using ProductCatalogue.Api.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ProductCatalogue.Api.Controllers
{
    [ApiController]
    [TypeFilter(typeof(ExceptionHandlerFilter))]
    [TypeFilter(typeof(ValidateProductIdFilter))]
    public class ProductsOptionsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsOptionsController> _logger;

        public ProductsOptionsController(IProductRepository repository,
            IMapper mapper, ILogger<ProductsOptionsController> logger)
        {
            _productRepository = repository ??
                 throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiRoutes.ProductOptions.GetAll)]
        public ActionResult<IEnumerable<ProductOptionResponse>> GetOptionsForProduct(Guid productId)
        {
            _logger.LogDebug($"Start - GetOptionsForProduct, params - productId : {productId}");

            var optionForProductFromRepo = _productRepository.GetProductOptions(productId);
            var optionForProductToReturn = 
                _mapper.Map<CollectionResponse<ProductOptionResponse>>(optionForProductFromRepo);

            var logReturnMessage = $"productId : {productId} {JsonConvert.SerializeObject(optionForProductToReturn)}";
            _logger.LogDebug($"End - GetOptionsForProduct, return - {logReturnMessage}");

            return Ok(optionForProductToReturn);
        }

        [HttpGet(ApiRoutes.ProductOptions.Get, Name = "GetOptionForProduct")]
        public ActionResult<ProductOptionResponse> GetOptionForProduct(Guid productId, Guid optionId)
        {
            _logger.LogDebug($"Start - GetOptionForProduct, params - productId : {productId} Id: {optionId}");

            var optionForProductFromRepo = _productRepository.GetProductOption(productId, optionId);
            if (optionForProductFromRepo == null)
                return NotFound();

            var optionForProductToReturn = _mapper.Map<ProductOptionResponse>(optionForProductFromRepo);

            var logReturnMessage = $"productId : {productId} {JsonConvert.SerializeObject(optionForProductToReturn)}";
            _logger.LogDebug($"End - GetOptionForProduct, return -{logReturnMessage}");

            return Ok(optionForProductToReturn);
        }

        [HttpPost(ApiRoutes.ProductOptions.Create)]
        public ActionResult<ProductOptionResponse> CreateOptionForProduct(
            Guid productId, CreateProductOptionRequest optionForProductToCreate)
        {
            var logParamsMessage = $"productId : {productId} {JsonConvert.SerializeObject(optionForProductToCreate)}";
            _logger.LogDebug($"Start - CreateOptionForProduct, params - {logParamsMessage}");

            var optionForProductEntity = _mapper.Map<ProductOption>(optionForProductToCreate);

            _productRepository.AddProductOption(productId, optionForProductEntity);
            _productRepository.Save();

            var optionForProductToReturn = _mapper.Map<ProductOptionResponse>(optionForProductEntity);

            var logReturnMessage = $"productId : {productId} {JsonConvert.SerializeObject(optionForProductToReturn)}";
            _logger.LogDebug($"End - CreateOptionForProduct, return - {logReturnMessage}");

            return CreatedAtRoute("GetOptionForProduct", 
                new { ProductId = productId, optionId = optionForProductEntity.Id },
                optionForProductToReturn);
        }

        [HttpPut(ApiRoutes.ProductOptions.Update)]
        public IActionResult UpdateOptionForProduct(Guid productId, Guid optionId,
            UpdateProductOptionRequest optionForProductToUpdate)
        {
            var logParamsMessage = $"productId : {productId} optionId : {optionId} {JsonConvert.SerializeObject(optionForProductToUpdate)}";
            _logger.LogDebug($"Start - UpdateOptionForProduct, params - {logParamsMessage}");

            var optionForProductFromRepo = _productRepository.GetProductOption(productId, optionId);

            if (optionForProductFromRepo == null)
                return NotFound();

            _mapper.Map(optionForProductToUpdate, optionForProductFromRepo);

            _productRepository.UpdateProductOption(optionForProductFromRepo);
            _productRepository.Save();

            var logReturnMessage = $"productId : {productId} optionId : {optionId} No Content";
            _logger.LogDebug($"Start - UpdateOptionForProduct, params - {logReturnMessage}");

            return NoContent();
        }

        [HttpDelete(ApiRoutes.ProductOptions.Delete)]
        public ActionResult DeleteOptionForProduct(Guid productId, Guid optionId)
        {
            var logParamsMessage = $"productId : {productId} optionId : {optionId}";
            _logger.LogDebug($"Start - DeleteOptionForProduct, params - {logParamsMessage}");

            var optionForProductFromRepo = _productRepository.GetProductOption(productId, optionId);

            if (optionForProductFromRepo == null)
                return NotFound();

            _productRepository.DeleteProductOption(optionForProductFromRepo);
            _productRepository.Save();

            var logReturnMessage = $"productId : {productId} optionId : {optionId} No Content";
            _logger.LogDebug($"Start - DeleteOptionForProduct, params - {logReturnMessage}");

            return NoContent();
        }
    }
}
