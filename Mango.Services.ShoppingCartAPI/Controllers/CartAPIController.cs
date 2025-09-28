using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ResponseDto response;
        public CartAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            response = new ResponseDto();
        }

        //TODO: Draw relations between all dtos and models related to cart
        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                                                .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    // Create new cart header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.Id;
                    CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                    _db.CartDetails.Add(cartDetails);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // If cart header exists, check for cart details
                    // If details for the product do not exist, create them

                    //TODO: Review this part AsNoTracking() how it can benfit us here

                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId 
                        && u.CartHeaderId == cartHeaderFromDb.Id);

                    if (cartDetailsFromDb == null)
                    {
                        //create cart details
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;
                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                        _db.CartDetails.Add(cartDetails);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update the count / cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;

                        //TODO: Review if we need to map all properties or just the count
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().Id = cartDetailsFromDb.Id;

                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                        _db.CartDetails.Update(cartDetails);
                        //_db.SaveChanges();
                        await _db.SaveChangesAsync();
                    }
                }
                response.Result = cartDto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
