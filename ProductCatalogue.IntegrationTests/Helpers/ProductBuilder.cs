using ProductCatalogue.Api.Models;
using System;
using System.Collections.Generic;

namespace ProductCatalogue.IntegrationTests.Helpers
{
    public class ProductBuilder
    {
        readonly Product product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Apple iPhone 6S",
            Description = "Newest mobile product from Apple.",
            Price = 1299.99M,
            DeliveryPrice = 15.99M,
            Options = new List<ProductOption>
                    {
                        new ProductOption { Id = Guid.NewGuid(), Name = "Black", Description = "Black Apple iPhone 6S" },
                        new ProductOption { Id = Guid.NewGuid(), Name = "White", Description = "White Apple iPhone 6S" },
                    }
        };

        public Product Build()
        {
            return product;
        }

        public ProductBuilder Name(string name)
        {
            product.Name = name;
            return this;
        }

        public ProductBuilder Description(string description)
        {
            product.Description = description;
            return this;
        }

        public ProductBuilder Price(decimal price)
        {
            product.Price = price;
            return this;
        }

        public ProductBuilder DeliveryPrice(decimal deliveryPrice)
        {
            product.DeliveryPrice = deliveryPrice;
            return this;
        }

        public ProductBuilder Options(List<ProductOption> options)
        {
            product.Options = options;
            return this;
        }
    }
}
