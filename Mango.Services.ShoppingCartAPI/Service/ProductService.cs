﻿using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        //private readonly ITokenProvider _tokenProvider;

        public ProductService(IHttpClientFactory httpClientFactory)//, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            //_tokenProvider = tokenProvider;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            /// "Product" is to get base address that is registered in Program.cs
            /// It will map correctly with base address of API we need to communicate
            /// instead accessing it here, we access from central place - DI container.
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync("/api/product"); 
            
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if(resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(
                    Convert.ToString(resp.Result));
            }

            return new List<ProductDto>();
        }
    }
}
