using FluentAssertions;
using NUnit.Framework;
using ProductCatalogue.Api.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ProductCatalogue.IntegrationTests.Products
{
    public class ProductsControllerDeleteActionShould : IntegrationTest
    {
        [Test]
        public async Task DeleteExistingProduct()
        {
            // Arrange
            var product = new ProductBuilder()
                        .Build();
            AddRecords(product);

            // Act            
            var response = await TestClient.DeleteAsync(
                new UrlFactory(ApiRoutes.Products.Delete).Create(product.Id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public async Task ReturnNotFoundError()
        {
            // Act            
            var response = await TestClient.DeleteAsync(
                new UrlFactory(ApiRoutes.Products.Delete).Create(Guid.NewGuid()));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }        
    }
}
