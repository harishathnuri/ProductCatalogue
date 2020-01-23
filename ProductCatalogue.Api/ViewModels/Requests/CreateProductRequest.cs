using System.Collections.Generic;

namespace ProductCatalogue.Api.ViewModels.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public ICollection<CreateProductOptionRequest> Options { get; set; }
                = new List<CreateProductOptionRequest>();
    }
}
