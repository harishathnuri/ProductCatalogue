using ProductCatalogue.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalogue.Api.Services
{
    public interface IProductRepository
    {
        Products GetProducts(GetAllProductsFilter filter);
        Product GetProduct(Guid productId);
        void AddProduct(Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);

        ProductOptions GetProductOptions(Guid productId);
        ProductOption GetProductOption(Guid productId, Guid productOptionId);
        void AddProductOption(Guid ProductId, ProductOption productOption);
        void DeleteProductOption(ProductOption productOption);
        void UpdateProductOption(ProductOption productOption);

        bool ProductExists(Guid productId);
        bool Save();
    }
}
