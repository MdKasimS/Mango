using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]//-> this is shown in Swagger
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db ;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]//-> internal name for URL
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> coupons = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]//-> internal name for URL
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(c=>c.Id==id);
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
                return _response;
        }

        [HttpGet]//-> internal name for URL
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _db.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.Result = StatusCodes.Status500InternalServerError;
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

      
    }
}
