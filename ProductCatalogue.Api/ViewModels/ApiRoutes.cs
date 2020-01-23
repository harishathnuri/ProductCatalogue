namespace ProductCatalogue.Api.ViewModels
{
    public class ApiRoutes
    {
        public const string Base = "api";
        public const string ProductOptionsBase = Base + "/products/{productId:guid}";

        public static class Products
        {
            public const string GetAll = Base + "/products";

            public const string Get = Base + "/products/{productId:guid}";

            public const string Create = Base + "/products";

            public const string Update = Base + "/products/{productId:guid}";

            public const string Delete = Base + "/products/{productId:guid}";
        }

        public static class ProductOptions
        {
            public const string GetAll = ProductOptionsBase + "/options";

            public const string Get = ProductOptionsBase + "/options/{optionId:guid}";

            public const string Create = ProductOptionsBase + "/options";

            public const string Update = ProductOptionsBase + "/options/{optionId:guid}";

            public const string Delete = ProductOptionsBase + "/options/{optionId:guid}";
        }
    }
}
