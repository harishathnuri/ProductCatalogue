using ProductCatalogue.Api.Models;
using System;

namespace ProductCatalogue.IntegrationTests
{
    public class ProductOptionBuilder
    {
        readonly ProductOption _option = new ProductOption
        { 
            Id = Guid.NewGuid(),
            Name = "Black",
            Description = "Black Apple iPhone 6S"
        };

        public ProductOption Build(Guid productId)
        {
            _option.ProductId = productId;
            return _option;
        }

        public ProductOptionBuilder Name(string name)
        {
            _option.Name = name;
            return this;
        }

        public ProductOptionBuilder Description(string description)
        {
            _option.Description = description;
            return this;
        }
    }
}
