using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.Extensions;
using ProductCatalogue.Api.Services;
using ProductCatalogue.Api.ViewModels.Responses;
using Serilog;
using System;

namespace ProductCatalogue.Api
{
    public class Startup
    {
        private readonly ILogger<Startup> logger;

        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            this.logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProductCatalogueDataContext>(
                options => options.UseSqlite(Configuration.GetConnectionString("ProductCatalogueAppConnection")));

            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            })
            .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>())
            .AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
            .AddApiBehaviorOptions();

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(Configuration["SwaggerGenOptions:name"],
                        new OpenApiInfo()
                        {
                            Title = Configuration["SwaggerGenOptions:title"],
                            Version = Configuration["SwaggerGenOptions:version"]
                        });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler(options => options.AddResponseDetails = UpdateApiErrorResponse);

            app.UseHsts();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwaggerForProductCatalogue(
                options => Configuration.Bind("SwaggerOptions", options));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        void UpdateApiErrorResponse(HttpContext context, Exception ex, FaultModel fault)
        {
            fault.Links = "https://gethelpforerror.com/";
        }
    }
}