using FluentAssertions;
using NUnit.Framework;
using ProductCatalogue.Api.ViewModels;
using ProductCatalogue.Api.ViewModels.Requests;
using ProductCatalogue.IntegrationTests.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductCatalogue.IntegrationTests.ProductOptions
{
    public class ProductOptionsControllerPutActionShould : ProductOptionsIntegrationTest
    {
        [Test]
        public async Task UpdateExistingProductOption()
        {
            // Arrange
            var productOption = new ProductOptionBuilder()
                        .Build(product.Id);
            AddRecords(productOption);
            var updateProductOptionRequest = 
                new UpdateProductOptionRequestBuilder().Build();

            // Act            
            var response = await TestClient.PutAsJsonAsync(
                new UrlFactory(ApiRoutes.ProductOptions.Update).Create(product.Id, productOption.Id),
                updateProductOptionRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Teardown
            RemoveRecords(productOption);
        }

        [Test]
        public async Task ReturnNotFoundError()
        {
            // Arrange
            var productOptionId = Guid.NewGuid();
            var updateProductOptionRequest = 
                new UpdateProductOptionRequestBuilder().Build();

            // Act            
            var response = await TestClient.PutAsJsonAsync(
                new UrlFactory(ApiRoutes.ProductOptions.Update).Create(product.Id, productOptionId),
                updateProductOptionRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task ReturnUnprocessableEntityErrorForEmptyAttributes()
        {
            // Arrange
            var productOption = new ProductOptionBuilder()
                                    .Build(product.Id);
            AddRecords(productOption);
            var updateProductOptionRequest =
                             new UpdateProductOptionRequestBuilder()
                                .Name(string.Empty)
                                .Description(string.Empty)
                                .Build();

            // Act            
            var response = await TestClient.PutAsJsonAsync(
                new UrlFactory(ApiRoutes.ProductOptions.Update).Create(product.Id, productOption.Id),
                updateProductOptionRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            // Teardown
            RemoveRecords(productOption);
        }

        [Test]
        public async Task ReturnUnprocessableEntityErrorWithLongAttributeValue()
        {
            // Arrange
            var productOption = new ProductOptionBuilder()
                                    .Build(product.Id);
            AddRecords(productOption);
            var updateProductOptionRequest =
                             new UpdateProductOptionRequestBuilder()
                                .Name(Constansts.LONG_PRODUCT_OPTION_NAME)
                                .Description(Constansts.LONG_PRODUCT_OPTION_DESCRIPTION)
                                .Build();

            // Act            
            var response = await TestClient.PutAsJsonAsync(
                new UrlFactory(ApiRoutes.ProductOptions.Update).Create(product.Id, productOption.Id),
                updateProductOptionRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            // Teardown
            RemoveRecords(productOption);
        }

        public class UpdateProductOptionRequestBuilder
        {
            readonly UpdateProductOptionRequest _request = new UpdateProductOptionRequest
            {
                Name = "Black",
                Description = "Black Apple iPhone 6S"
            };

            public UpdateProductOptionRequest Build()
            {
                return _request;
            }

            public UpdateProductOptionRequestBuilder Name(string name)
            {
                _request.Name = name;
                return this;
            }

            public UpdateProductOptionRequestBuilder Description(string description)
            {
                _request.Description = description;
                return this;
            }
        }
    }
}
