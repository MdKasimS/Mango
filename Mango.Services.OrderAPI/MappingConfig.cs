using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;

namespace Mango.Services.OrderAPI
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
                config.CreateMap<OrderHeaderDto, OrderHeader>().ReverseMap();
                config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
                //config.CreateMap<Product, ProductDto>();
            }, loggerFactory);
            return mappingConfig;
        }
    }
}