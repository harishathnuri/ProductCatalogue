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

namespace ProductCatalogue.Api.Controllers
{
    [ApiController]
    [TypeFilter(typeof(ValidateProductId))]
    public class ProductsOptionsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsOptionsController(IProductRepository repository, IMapper mapper)
        {
            _productRepository = repository;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.ProductOptions.GetAll)]
        public ActionResult<IEnumerable<ProductOptionResponse>> GetOptionsForProduct(Guid productId)
        {
            var optionForProductFromRepo = _productRepository.GetProductOptions(productId);
            var optionForProductToReturn = 
                _mapper.Map<CollectionResponse<ProductOptionResponse>>(optionForProductFromRepo);

            return Ok(optionForProductToReturn);
        }

        [HttpGet(ApiRoutes.ProductOptions.Get, Name = "GetOptionForProduct")]
        public ActionResult<ProductOptionResponse> GetOptionForProduct(Guid productId, Guid optionId)
        {
            var optionForProductFromRepo = _productRepository.GetProductOption(productId, optionId);
            if (optionForProductFromRepo == null)
                return NotFound();

            var optionForProductToReturn = _mapper.Map<ProductOptionResponse>(optionForProductFromRepo);

            return Ok(optionForProductToReturn);
        }

        [HttpPost(ApiRoutes.ProductOptions.Create)]
        public ActionResult<ProductOptionResponse> CreateOptionForProduct(
            Guid productId, CreateProductOptionRequest optionForProductToCreate)
        {
            var optionForProductEntity = _mapper.Map<ProductOption>(optionForProductToCreate);

            _productRepository.AddProductOption(productId, optionForProductEntity);
            _productRepository.Save();

            var optionForProductToReturn = _mapper.Map<ProductOptionResponse>(optionForProductEntity);

            return CreatedAtRoute("GetOptionForProduct", 
                new { ProductId = productId, optionId = optionForProductEntity.Id },
                optionForProductToReturn);
            
        }

        [HttpPut(ApiRoutes.ProductOptions.Update)]
        public IActionResult UpdateOptionForProduct(Guid productId, Guid optionId,
            UpdateProductOptionRequest optionForProductToUpdate)
        {
            var optionForProductFromRepo = _productRepository.GetProductOption(productId, optionId);

            if (optionForProductFromRepo == null)
                return NotFound();

            _mapper.Map(optionForProductToUpdate, optionForProductFromRepo);

            _productRepository.UpdateProductOption(optionForProductFromRepo);
            _productRepository.Save();

            return NoContent();
        }

        [HttpDelete(ApiRoutes.ProductOptions.Delete)]
        public ActionResult DeleteOptionForProduct(Guid productId, Guid optionId)
        {
            var optionForProductFromRepo = _productRepository.GetProductOption(productId, optionId);

            if (optionForProductFromRepo == null)
                return NotFound();

            _productRepository.DeleteProductOption(optionForProductFromRepo);
            _productRepository.Save();

            return NoContent();
        }
    }
}
