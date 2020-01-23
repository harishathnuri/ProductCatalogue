using FluentAssertions;
using NUnit.Framework;
using ProductCatalogue.Api.ViewModels;
using ProductCatalogue.Api.ViewModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductCatalogue.IntegrationTests.Products
{
    public class ProductsControllerGetActionsShould : IntegrationTest
    {
        [Test]
        public async Task ReturnEmptyResponse()
        {
            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Products.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<CollectionResponse<ProductResponse>>()).Items.Should().BeEmpty();
        }

        [Test]
        public async Task ReturnOneProductInformation()
        {
            // Arrange
            var product = new ProductBuilder()
                        .Build();
            AddRecords(product);

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Products.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<CollectionResponse<ProductResponse>>()).Items.Should().HaveCount(1);

            // Teardown
            RemoveRecords(product);
        }

        [Test]
        public async Task ReturnFilteredResultByName()
        {
            // Arrange
            var apple6s = "Apple iPhone 6S";
            var product = new ProductBuilder()
                        .Name(apple6s)
                        .Build();
            AddRecords(product);
            
            // Act
            var response = await TestClient.GetAsync($"{ApiRoutes.Products.GetAll}/?name={apple6s}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = (await response.Content.ReadAsAsync<CollectionResponse<ProductResponse>>()).Items.ToList();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be(apple6s);

            // Teardown
            RemoveRecords(product);
        }

        [Test]
        public async Task ReturnFilteredResultByNameHavingZeroCount()
        {
            // Arrange
            var apple6s = "Apple iPhone 6S";
            var apple7 = "Apple iPhone 7";
            var product = new ProductBuilder()
                        .Name(apple6s)
                        .Build();
            AddRecords(product);

            // Act
            var response = await TestClient.GetAsync($"{ApiRoutes.Products.GetAll}/?name={apple7}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = (await response.Content.ReadAsAsync<CollectionResponse<ProductResponse>>()).Items;
            result.Should().HaveCount(0);

            // Teardown
            RemoveRecords(product);
        }

        [Test]
        public async Task ReturnOneProductWithAGivenId()
        {
            // Arrange
            var product = new ProductBuilder().Build();
            AddRecords(product);

            // Act
            var response = await TestClient.GetAsync(
                new UrlFactory(ApiRoutes.Products.Get).Create(product.Id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = (await response.Content.ReadAsAsync<ProductResponse>());
            result.Id.Should().Be(product.Id);

            // Teardown
            RemoveRecords(product);
        }

        [Test]
        public async Task ReturnNotFoundWithGivenId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = new ProductBuilder().Build();
            AddRecords(product);

            // Act
            var response = await TestClient.GetAsync(
                new UrlFactory(ApiRoutes.Products.Get).Create(id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            // Teardown
            RemoveRecords(product);
        }
    }
}
