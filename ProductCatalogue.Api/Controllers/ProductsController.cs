using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductCatalogue.Api.Filters;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;
using ProductCatalogue.Api.ViewModels;
using ProductCatalogue.Api.ViewModels.Requests;
using ProductCatalogue.Api.ViewModels.Requests.Queries;
using ProductCatalogue.Api.ViewModels.Responses;
using System;
using System.Collections.Generic;

namespace ProductCatalogue.Api.Controllers
{
    [ApiController]
    [TypeFilter(typeof(ExceptionHandlerFilter))]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository repository,
            IMapper mapper, ILogger<ProductsController> logger)
        {
            _productRepository = repository ??
                 throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiRoutes.Products.GetAll)]
        public ActionResult<IEnumerable<ProductResponse>> GetProducts(
            [FromQuery] GetAllProductsQuery getAllProductsQuery)
        {
            _logger.LogDebug($"Start - GetProducts, params - {JsonConvert.SerializeObject(getAllProductsQuery)}");

            var productsFilter = _mapper.Map<GetAllProductsFilter>(getAllProductsQuery);

            var productsFromRepo = _productRepository.GetProducts(productsFilter);

            var productsToReturn = _mapper.Map<CollectionResponse<ProductResponse>>(productsFromRepo);

            _logger.LogDebug($"End - GetProducts, return - {JsonConvert.SerializeObject(productsFromRepo)}");

            return Ok(productsToReturn);
        }

        [HttpGet(ApiRoutes.Products.Get, Name = "GetProduct")]
        public ActionResult<ProductResponse> GetProduct(Guid productId)
        {
            _logger.LogDebug($"Start - GetProduct, params - {productId}");

            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
                return NotFound();

            var productToReturn = _mapper.Map<ProductResponse>(productFromRepo);

            _logger.LogDebug($"End - GetProduct, return - {productId}");

            return Ok(productToReturn);
        }

        [HttpPost(ApiRoutes.Products.Create)]
        public ActionResult<ProductResponse> CreateProduct(CreateProductRequest productForCreation)
        {
            _logger.LogDebug($"Start - CreateProduct, params - {JsonConvert.SerializeObject(productForCreation)}");

            var productEntity = _mapper.Map<Product>(productForCreation);

            _productRepository.AddProduct(productEntity);
            _productRepository.Save();

            var productToReturn = _mapper.Map<ProductResponse>(productEntity);

            _logger.LogDebug($"Start - CreateProduct, return - {productToReturn}");

            return CreatedAtRoute("GetProduct", new { productId = productEntity.Id }, productToReturn);
        }

        [HttpPut(ApiRoutes.Products.Update)]
        public IActionResult UpdateProduct(Guid productId, UpdateProductRequest productForUpdate)
        {
            _logger.LogDebug($"Start - UpdateProduct, params - Id {productId} {productForUpdate}");

            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
                return NotFound();

            _mapper.Map(productForUpdate, productFromRepo);

            _productRepository.UpdateProduct(productFromRepo);
            _productRepository.Save();

            _logger.LogDebug($"End - UpdateProduct, return - No Content");

            return NoContent();
        }

        [HttpDelete(ApiRoutes.Products.Delete)]
        public ActionResult DeleteProduct(Guid productId)
        {
            _logger.LogDebug($"Start - DeleteProduct, params - Id {productId}");

            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
                return NotFound();

            _productRepository.DeleteProduct(productFromRepo);
            _productRepository.Save();

            _logger.LogDebug($"End - DeleteProduct, return - No Content");

            return NoContent();
        }
    }
}
