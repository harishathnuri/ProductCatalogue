using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _productRepository = repository ??
                 throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(ApiRoutes.Products.GetAll)]
        public ActionResult<IEnumerable<ProductResponse>> GetProducts([FromQuery] GetAllProductsQuery getAllProductsQuery)
        {
            var productsFilter = _mapper.Map<GetAllProductsFilter>(getAllProductsQuery);

            var productsFromRepo = _productRepository.GetProducts(productsFilter);

            var productsToReturn = _mapper.Map<CollectionResponse<ProductResponse>>(productsFromRepo);

            return Ok(productsToReturn);
        }

        [HttpGet(ApiRoutes.Products.Get, Name = "GetProduct")]
        public ActionResult<ProductResponse> GetProduct(Guid productId)
        {
            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
                return NotFound();

            var productToReturn = _mapper.Map<ProductResponse>(productFromRepo);

            return Ok(productToReturn);
        }

        [HttpPost(ApiRoutes.Products.Create)]
        public ActionResult<ProductResponse> CreateProduct(CreateProductRequest productForCreation)
        {
            var productEntity = _mapper.Map<Product>(productForCreation);

            _productRepository.AddProduct(productEntity);
            _productRepository.Save();

            var productToReturn = _mapper.Map<ProductResponse>(productEntity);

            return CreatedAtRoute("GetProduct", new { productId = productEntity.Id }, productToReturn);
        }

        [HttpPut(ApiRoutes.Products.Update)]
        public IActionResult UpdateProduct(Guid productId, UpdateProductRequest productForUpdate)
        {
            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
                return NotFound();

            _mapper.Map(productForUpdate, productFromRepo);

            _productRepository.UpdateProduct(productFromRepo);
            _productRepository.Save();

            return NoContent();
        }

        [HttpDelete(ApiRoutes.Products.Delete)]
        public ActionResult DeleteProduct(Guid productId)
        {
            var productFromRepo = _productRepository.GetProduct(productId);

            if (productFromRepo == null)
                return NotFound();

            _productRepository.DeleteProduct(productFromRepo);
            _productRepository.Save();

            return NoContent();
        }
    }
}
