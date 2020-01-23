using System.Collections.Generic;

namespace ProductCatalogue.Api.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; } = new List<Product>();

        public Products(List<Product> items)
        {
            Items = items;
        }
    }
}
