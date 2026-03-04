using AutoMapper;

using Mango.MessageBus;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Service.IService;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IMessageProducer _messageBus;

        /// <summary>
        /// Dont Make it private readonly, else it will be null.
        /// </summary>
        private IProductService _productService;
        private IConfiguration _configuration;
        private readonly ResponseDto _response;

        public OrderAPIController(AppDbContext db, IMapper mapper
                                , IProductService productService
                                , IMessageProducer messageBus
                                , IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _messageBus = messageBus;
            _productService = productService;
            _configuration = configuration;
            _response = new ResponseDto();
            _configuration = configuration;
        }
    }
}
