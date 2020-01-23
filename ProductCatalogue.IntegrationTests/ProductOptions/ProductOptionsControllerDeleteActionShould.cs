using FluentAssertions;
using NUnit.Framework;
using ProductCatalogue.Api.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;
namespace ProductCatalogue.IntegrationTests.ProductOptions
{
    public class ProductOptionsControllerDeleteActionShould : ProductOptionsIntegrationTest
    {
        [Test]
        public async Task DeleteExistingProductOption()
        {
            // Arrange
            var productOption = new ProductOptionBuilder()
                       .Build(product.Id);
            AddRecords(productOption);

            // Act            
            var response = await TestClient.DeleteAsync(
                new UrlFactory(ApiRoutes.ProductOptions.Delete).Create(product.Id, productOption.Id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public async Task ReturnNotFoundError()
        {
            //Arrange
            var productOptionId = Guid.NewGuid();

            // Act            
            var response = await TestClient.DeleteAsync(
               new UrlFactory(ApiRoutes.ProductOptions.Delete).Create(product.Id, productOptionId));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
