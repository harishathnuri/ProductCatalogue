using FluentAssertions;
using NUnit.Framework;
using ProductCatalogue.Api.ViewModels;
using ProductCatalogue.Api.ViewModels.Requests;
using ProductCatalogue.Api.ViewModels.Responses;
using ProductCatalogue.IntegrationTests.Helpers;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductCatalogue.IntegrationTests.Products
{
    public class ProductsControllerPostActionShould : IntegrationTest
    {
        [Test]
        public async Task CreateNewProduct()
        {
            // Arrange
            var createProductRequest = new CreateProductRequestBuilder().Build();

            // Act            
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Products.Create, createProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            var createdProduct = (await response.Content.ReadAsAsync<ProductResponse>());
            
            createdProduct.Should().NotBeNull();

            createdProduct.Id.Should().NotBeEmpty();
            createdProduct.Name.Should().Be(createProductRequest.Name);
            createdProduct.Description.Should().Be(createProductRequest.Description);
            createdProduct.Price.Should().Be(createProductRequest.Price);
            createdProduct.DeliveryPrice.Should().Be(createProductRequest.DeliveryPrice);
        }

        [Test]
        public async Task ReturnUnprocessableEntityErrorForEmptyAttributes()
        {
            // Arrange
            var createProductRequest = new CreateProductRequestBuilder()
                                .Name(string.Empty)
                                .Description(string.Empty)
                                .Price(0M)
                                .DeliveryPrice(0M)
                                .Build();

            // Act            
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Products.Create, createProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Test]
        public async Task ReturnUnprocessableEntityErrorWithLongAttributeValue()
        {
            // Arrange
            var createProductRequest = new CreateProductRequestBuilder()
                                .Name(Constansts.LONG_PRODUCT_NAME)
                                .Description(Constansts.LONG_PRODUCT_DESCRIPTION)
                                .Build();

            // Act            
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Products.Create, createProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Test]
        public async Task ReturnUnprocessableEntityErrorWithZeroPriceAndDelivery()
        {
            // Arrange
            var createProductRequest = new CreateProductRequestBuilder()
                                .Price(0M)
                                .DeliveryPrice(0M)
                                .Build();

            // Act            
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Products.Create, createProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }


        public class CreateProductRequestBuilder
        {
            CreateProductRequest request = new CreateProductRequest
                {
                    Name = "Apple iPhone 6S",
                    Description = "Newest mobile product from Apple.",
                    Price = 1299.99M,
                    DeliveryPrice = 15.99M,
                    Options = new List<CreateProductOptionRequest>
                    {
                        new CreateProductOptionRequest { Name = "Black", Description = "Black Apple iPhone 6S" },
                        new CreateProductOptionRequest { Name = "White", Description = "White Apple iPhone 6S" },
                    }
                };

            public CreateProductRequest Build()
            {
                return request;
            }

            public CreateProductRequestBuilder Name(string name)
            {
                request.Name = name;
                return this;
            }

            public CreateProductRequestBuilder Description(string description)
            {
                request.Description = description;
                return this;
            }

            public CreateProductRequestBuilder Price(decimal price)
            {
                request.Price = price;
                return this;
            }

            public CreateProductRequestBuilder DeliveryPrice(decimal deliveryPrice)
            {
                request.DeliveryPrice = deliveryPrice;
                return this;
            }

            public CreateProductRequestBuilder Options(List<CreateProductOptionRequest> options)
            {
                request.Options = options;
                return this;
            }
        }
    }
}
