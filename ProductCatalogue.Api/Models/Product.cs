using System;
using System.Collections.Generic;

namespace ProductCatalogue.Api.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public ICollection<ProductOption> Options { get; set; } = new List<ProductOption>();
    }
}
