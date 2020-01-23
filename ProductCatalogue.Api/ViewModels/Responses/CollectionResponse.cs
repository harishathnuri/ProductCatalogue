using System.Collections.Generic;

namespace ProductCatalogue.Api.ViewModels.Responses
{
    public class CollectionResponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
    }
}
