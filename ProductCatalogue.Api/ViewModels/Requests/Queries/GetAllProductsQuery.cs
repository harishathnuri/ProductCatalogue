using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogue.Api.ViewModels.Requests.Queries
{
    public class GetAllProductsQuery
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
    }
}
