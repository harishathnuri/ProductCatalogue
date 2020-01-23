using System;

namespace ProductCatalogue.IntegrationTests.Helpers
{
    public class UrlFactory
    {
        string _url = string.Empty;
        public UrlFactory(string url)
        {
            _url = url;
        }
        public string Create(Guid productId)
        {
            return _url.Replace("{productId:guid}", $"{productId.ToString()}");
        }

        public string Create(Guid productId, Guid optionId)
        {
            var url = _url.Replace("{productId:guid}", $"{productId.ToString()}");
            return url.Replace("{optionId:guid}", $"{optionId.ToString()}");
        }
    }
}
