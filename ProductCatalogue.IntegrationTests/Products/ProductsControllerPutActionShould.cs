using FluentAssertions;
using NUnit.Framework;
using ProductCatalogue.Api.ViewModels;
using ProductCatalogue.Api.ViewModels.Requests;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductCatalogue.IntegrationTests.Products
{
    public class ProductsControllerPutActionShould : IntegrationTest
    {
        [Test]
        public async Task UpdateExistingProduct()
        {
            // Arrange
            var product = new ProductBuilder()
                        .Build();
            AddRecords(product);
            var updateProductRequest = new UpdateProductRequestBuilder().Build();

            // Act            
            var response = await TestClient.PutAsJsonAsync(
                new UrlFactory(ApiRoutes.Products.Update).Create(product.Id),
                updateProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Teardown
            RemoveRecords(product);
        }

        [Test]
        public async Task ReturnNotFoundError()
        {
            // Arrange
            var updateProductRequest = new UpdateProductRequestBuilder()
                                .Build();

            // Act            
            var response = await TestClient.PutAsJsonAsync(
                new UrlFactory(ApiRoutes.Products.Update).Create(Guid.NewGuid()),
                updateProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task ReturnUnprocessableEntityErrorForEmptyAttributes()
        {
            // Arrange
            var product = new ProductBuilder()
                        .Build();
            AddRecords(product);
            var updateProductRequest = new UpdateProductRequestBuilder()
                                .Name(string.Empty)
                                .Description(string.Empty)
                                .Build();

            // Act            
            var response = await TestClient.PutAsJsonAsync(
                new UrlFactory(ApiRoutes.Products.Update).Create(product.Id),
                updateProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            // Teardown
            RemoveRecords(product);
        }

        [Test]
        public async Task ReturnUnprocessableEntityErrorWithLongAttributeValue()
        {
            // Arrange
            var product = new ProductBuilder()
                        .Build();
            AddRecords(product);
            var updateProductRequest = new UpdateProductRequestBuilder()
                                .Name(Constansts.LONG_PRODUCT_NAME)
                                .Description(Constansts.LONG_PRODUCT_DESCRIPTION)
                                .Build();

            // Act            
            var response = await TestClient.PutAsJsonAsync(
                new UrlFactory(ApiRoutes.Products.Update).Create(product.Id),
                updateProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            // Teardown
            RemoveRecords(product);
        }

        public class UpdateProductRequestBuilder
        {
            readonly UpdateProductRequest _request = new UpdateProductRequest
            {
                Name = "Apple iPhone 6S",
                Description = "Newest mobile product from Apple.",
            };

            public UpdateProductRequest Build()
            {
                return _request;
            }

            public UpdateProductRequestBuilder Name(string name)
            {
                _request.Name = name;
                return this;
            }

            public UpdateProductRequestBuilder Description(string description)
            {
                _request.Description = description;
                return this;
            }
        }
    }
}
