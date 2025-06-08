using Microsoft.AspNetCore.Mvc;
using Service;
using DataModel;
using Model;

namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController
    {
        private IDiscountService discountService;

        public DiscountController(IDiscountService discountService)
        {
            this.discountService = discountService;
        }


        [HttpGet("discounts")]
        public List<DiscountDto> GetDiscountDto()
        {
            return discountService.GetDiscountDto();
        }

        [HttpGet("discountsPaged")]
        public async Task<PagedResult<DiscountDto>> GetDiscounts(int pageNumber, int pageSize, string? search = null)
        {
            return await discountService.GetDiscountsPagedAsync(pageNumber, pageSize, search);
        }


        [HttpGet("{id}")]
        public DiscountDto GetDiscount(int id)
        {
            return this.discountService.GetDiscountDto(id);
        }
        [HttpPost("add")]
        public bool PostDiscount(DiscountDto discountDto)
        {
            var createrDiscount = discountService.AddDiscountDto(discountDto);
            return createrDiscount;
        }
        [HttpPut("edit")]
        public bool EditDiscount(DiscountDto discountDto)
        {
            return discountService.UpdateDiscountDto(discountDto);
        }

        [HttpDelete("delete/{id}")]
        public bool DeleteDiscount(int id)
        {
             return discountService.DeleteDiscountDto(id);
        }
    }
}
