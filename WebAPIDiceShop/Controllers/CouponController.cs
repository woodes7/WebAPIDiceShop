using DataModel;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService couponService;

        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }

        [HttpGet("cupons")]
        public List<CouponDto> GetCoupons()
        {
            return couponService.GetCoupons();
        }

     
        [HttpGet("cuponsPaged")]
        public async Task<PagedResult<CouponDto>> GetCouponsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            return await couponService.GetCouponsPagedAsync(pageNumber, pageSize, search);           
        }
 
        [HttpGet("{id}")]
        public ActionResult<CouponDto> GetById(int id)
        {
            var coupon = couponService.GetCouponById(id);

            if (coupon == null)
                return NotFound(); // Devuelve 404 si no existe

            return Ok(coupon); // Devuelve 200 con el objeto si sí existe
        }

        [HttpPost("add")]
        public bool Create([FromBody] CouponDto couponDto)
        {
            var createdCoupon = couponService.AddCoupon(couponDto);
            return createdCoupon;
        }

        [HttpPut("edit")]
        public bool UpdateCategory(CouponDto couponDto)
        {
            return couponService.UpdateCoupon(couponDto);
        }


        [HttpDelete("delete/{id}")]
        public bool Delete(int id)
        {
            var result = couponService.DeleteCoupon(id);
            return result;
        }

        [HttpGet("code/{code}")]
        public ActionResult<CouponDto> GetCouponByCode(string code)
        {
            var coupon = couponService.GetCouponByCode(code);
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }

        [HttpPost("use/{id}")]
        public IActionResult UseCoupon(int id)
        {
            var success = couponService.UseCoupon(id);
            if (!success) return BadRequest("Cupón no válido o ya utilizado.");
            return Ok("Cupón marcado como usado.");
        }


    }

}
