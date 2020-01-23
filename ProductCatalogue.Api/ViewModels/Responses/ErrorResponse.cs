using System.Collections.Generic;

namespace ProductCatalogue.Api.ViewModels.Responses
{
    public class ErrorResponse
    {
        public ICollection<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
