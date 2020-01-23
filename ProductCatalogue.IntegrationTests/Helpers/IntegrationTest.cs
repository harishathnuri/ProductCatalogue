using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProductCatalogue.Api;
using System;
using ProductCatalogue.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using ProductCatalogue.Api.ViewModels.Responses;
using ProductCatalogue.Api.ViewModels.Requests;
using System.Diagnostics;
using System.Collections.Generic;

namespace ProductCatalogue.IntegrationTests.Helpers
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider _serviceProvider;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => 
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(ProductCatalogueDataContext));
                        services.AddDbContext<ProductCatalogueDataContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                    });
                });

            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        protected async Task<ProductResponse> CreatePostAsync(CreateProductRequest request)
        {
            var response = await TestClient.PostAsJsonAsync("", request);
            return (await response.Content.ReadAsAsync<ProductResponse>());
        }

        protected void AddRecords<T>(T record)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ProductCatalogueDataContext>();
                context.Add(record);
                context.SaveChanges();
            }
        }

        protected void RemoveRecords<T>(T record)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ProductCatalogueDataContext>();
                context.Remove(record);
                context.SaveChanges();
            }
        }

        protected void AddRecords<T>(List<T> records)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ProductCatalogueDataContext>();
                foreach (var record in records)
                {
                    context.Add(record);
                }
                context.SaveChanges();
            }
        }

        protected void RemoveRecords<T>(List<T> records)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ProductCatalogueDataContext>();
                foreach (var record in records)
                {
                    context.Remove(record);
                }
                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ProductCatalogueDataContext>();
                context.Database.EnsureDeleted();
            }
        }
    }
}