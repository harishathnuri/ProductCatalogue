using NUnit.Framework;
using ProductCatalogue.Api.Models;
using ProductCatalogue.IntegrationTests.Helpers;
using System.Collections.Generic;

namespace ProductCatalogue.IntegrationTests.ProductOptions
{
    public class ProductOptionsIntegrationTest : IntegrationTest
    {
        protected Product product = null;

        [SetUp]
        public void Setup()
        {
            var product = new ProductBuilder()
                        .Options(new List<ProductOption>())
                        .Build();
            AddRecords(product);
            this.product = product;
        }

        [TearDown]
        public void TearDown()
        {
            // Teardown
            RemoveRecords(product);
        }
    }
}
