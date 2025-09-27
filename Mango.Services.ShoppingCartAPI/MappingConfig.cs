using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });

            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
                //config.CreateMap<Product, ProductDto>();
            }, loggerFactory);
            return mappingConfig;
        }
    }
}