using FluentAssertions;
using NUnit.Framework;
using ProductCatalogue.Api.ViewModels;
using ProductCatalogue.Api.ViewModels.Requests;
using ProductCatalogue.Api.ViewModels.Responses;
using ProductCatalogue.IntegrationTests.Helpers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductCatalogue.IntegrationTests.ProductOptions
{
    public class ProductOptionsControllerPostActionShould : ProductOptionsIntegrationTest
    {
        [Test]
        public async Task CreateNewProductOption()
        {
            // Arrange
            var createProductOptionRequest = new CreateProductOptionRequestBuilder().Build();

            // Act            
            var response = await TestClient.PostAsJsonAsync(
                 new UrlFactory(ApiRoutes.ProductOptions.Create).Create(product.Id),
                 createProductOptionRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            var createdProductOption = (await response.Content.ReadAsAsync<ProductOptionResponse>());

            createdProductOption.Should().NotBeNull();

            createdProductOption.Id.Should().NotBeEmpty();
            createdProductOption.Name.Should().Be(createProductOptionRequest.Name);
            createdProductOption.Description.Should().Be(createProductOptionRequest.Description);
            createdProductOption.ProductId.Should().Be(product.Id);
        }

        [Test]
        public async Task ReturnUnprocessableEntityErrorForEmptyAttributes()
        {
            // Arrange
            var createProductOptionRequest = new CreateProductOptionRequestBuilder()
                                .Name(string.Empty)
                                .Description(string.Empty)
                                .Build();

            // Act            
            var response = await TestClient.PostAsJsonAsync(
                 new UrlFactory(ApiRoutes.ProductOptions.Create).Create(product.Id),
                 createProductOptionRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Test]
        public async Task ReturnUnprocessableEntityErrorWithLongAttributeValue()
        {
            // Arrange
            var createProductOptionRequest = new CreateProductOptionRequestBuilder()
                                .Name(Constansts.LONG_PRODUCT_OPTION_NAME)
                                .Description(Constansts.LONG_PRODUCT_OPTION_DESCRIPTION)
                                .Build();

            // Act            
            var response = await TestClient.PostAsJsonAsync(
                 new UrlFactory(ApiRoutes.ProductOptions.Create).Create(product.Id),
                 createProductOptionRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        public class CreateProductOptionRequestBuilder
        {
            CreateProductOptionRequest _request = new CreateProductOptionRequest
            {
                Name = "Black",
                Description = "Black Apple iPhone 6S"
            };

            public CreateProductOptionRequest Build()
            {                
                return _request;
            }

            public CreateProductOptionRequestBuilder Name(string name)
            {
                _request.Name = name;
                return this;
            }

            public CreateProductOptionRequestBuilder Description(string description)
            {
                _request.Description = description;
                return this;
            }
        }
    }
}
