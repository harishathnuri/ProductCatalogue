using FluentAssertions;
using NUnit.Framework;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.ViewModels;
using ProductCatalogue.Api.ViewModels.Responses;
using ProductCatalogue.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductCatalogue.IntegrationTests.ProductOptions
{
    public class ProductOptionsControllerGetActionsShould : ProductOptionsIntegrationTest
    {
        [Test]
        public async Task ReturnEmptyResponse()
        {
            // Act
            var response = await TestClient.GetAsync(
                new UrlFactory(ApiRoutes.ProductOptions.GetAll).Create(product.Id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<CollectionResponse<ProductOptionResponse>>()).Items.Should().BeEmpty();
        }

        [Test]
        public async Task ReturnFourProductOptionsForGivenProduct()
        {
            // Arrange
            var productOptions = new List<ProductOption>
            {
                new ProductOptionBuilder().Build(product.Id),
                new ProductOptionBuilder().Name("White").Build(product.Id),
                new ProductOptionBuilder().Name("Grey").Build(product.Id),
                new ProductOptionBuilder().Name("Rose Gold").Build(product.Id),
            };
            AddRecords(productOptions);

            // Act
            var response = await TestClient.GetAsync(
                new UrlFactory(ApiRoutes.ProductOptions.GetAll).Create(product.Id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<CollectionResponse<ProductOptionResponse>>()).Items.Should().HaveCount(4);

            // Teardown
            RemoveRecords(productOptions);
        }

        [Test]
        public async Task ReturnOneProductOptionWithAGivenId()
        {
            // Arrange
            var productOption = new ProductOptionBuilder().Build(product.Id);
            AddRecords(productOption);

            // Act
            var response = await TestClient.GetAsync(
                new UrlFactory(ApiRoutes.ProductOptions.Get).Create(product.Id, productOption.Id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = (await response.Content.ReadAsAsync<ProductOptionResponse>());
            result.Id.Should().Be(productOption.Id);

            // Teardown
            RemoveRecords(productOption);
        }

        [Test]
        public async Task ReturnNotFoundWithGivenId()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            var productOption = new ProductOptionBuilder().Build(product.Id);
            AddRecords(productOption);

            // Act
            var response = await TestClient.GetAsync(
                new UrlFactory(ApiRoutes.ProductOptions.Get).Create(product.Id, optionId));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            // Teardown
            RemoveRecords(productOption);
        }

        [Test]
        public async Task ReturnNotFoundWithGivenProductId()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            // Act
            var response = await TestClient.GetAsync(
                new UrlFactory(ApiRoutes.ProductOptions.Get).Create(productId, optionId));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
