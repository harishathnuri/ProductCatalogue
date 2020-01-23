using System;

namespace ProductCatalogue.Api.ViewModels.Responses
{
    public class ProductOptionResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid ProductId { get; set; }
    }
}
