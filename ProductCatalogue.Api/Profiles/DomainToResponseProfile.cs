using AutoMapper;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.ViewModels.Responses;

namespace ProductCatalogue.Api.Profiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductOption, ProductOptionResponse>();
            CreateMap<Products, CollectionResponse<ProductResponse>>();
            CreateMap<ProductOptions, CollectionResponse<ProductOptionResponse>>();
        }
    }
}
