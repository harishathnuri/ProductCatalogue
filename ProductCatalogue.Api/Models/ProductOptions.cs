using System.Collections.Generic;

namespace ProductCatalogue.Api.Models
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; } = new List<ProductOption>();

        public ProductOptions(List<ProductOption> items)
        {
            Items = items;
        }
    }
}
