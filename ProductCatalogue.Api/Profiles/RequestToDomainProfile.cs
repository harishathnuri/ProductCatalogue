using AutoMapper;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.ViewModels.Requests;
using ProductCatalogue.Api.ViewModels.Requests.Queries;

namespace ProductCatalogue.Api.Profiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<CreateProductOptionRequest, ProductOption>();
            CreateMap<UpdateProductOptionRequest, ProductOption>();
            CreateMap<UpdateProductRequest, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsFilter>();
        }
    }
}
