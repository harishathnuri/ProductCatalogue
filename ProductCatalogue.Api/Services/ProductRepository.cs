using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogue.Api.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductCatalogueDataContext _context;

        public ProductRepository(ProductCatalogueDataContext context)
        {
            _context = context;
        }

        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.Id = Guid.NewGuid();

            foreach (var option in product.Options)
            {
                option.Id = Guid.NewGuid();
            }

            _context.Products.Add(product);
        }

        public void AddProductOption(Guid productId, ProductOption productOption)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            if (productOption == null)
            {
                throw new ArgumentNullException(nameof(productOption));
            }

            productOption.ProductId = productId;
            productOption.Id = Guid.NewGuid();
            _context.ProductOptions.Add(productOption);
        }

        public void DeleteProduct(Product product)
        {
            var options = GetProductOptions(product.Id);
            _context.ProductOptions.RemoveRange(options.Items);
            _context.Products.Remove(product);
        }

        public void DeleteProductOption(ProductOption productOption)
        {
            _context.ProductOptions.Remove(productOption);
        }

        public Product GetProduct(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public ProductOption GetProductOption(Guid productId, Guid productOptionId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            if (productOptionId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productOptionId));
            }

            return _context.ProductOptions
              .Where(o => o.ProductId == productId && o.Id == productOptionId).FirstOrDefault();
        }

        public ProductOptions GetProductOptions(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }
            var optionsToReturn = _context.ProductOptions
                        .Where(o => o.ProductId == productId)
                        .OrderBy(o => o.Name).ToList() ?? new List<ProductOption>();
            var optionsForProduct = new ProductOptions(optionsToReturn);

            return optionsForProduct;
        }

        public Products GetProducts(GetAllProductsFilter filter = null)
        {
            var queryable = _context.Products.AsQueryable();

            if (filter != null && !string.IsNullOrEmpty(filter?.Name))
            {
                queryable = queryable.Where(x => x.Name == filter.Name);
            }

            var products = new Products(queryable.OrderBy(p => p.Name).ToList() ?? new List<Product>()); 
            
            return products;
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateProduct(Product product)
        {
            
        }

        public void UpdateProductOption(ProductOption productOption)
        {
            
        }

        public bool ProductExists(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            return _context.Products.Any(a => a.Id == productId);
        }       
    }
}
