using DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPIDiceShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService productReviewService;

        public ProductReviewController(IProductReviewService productReviewService)
        {
            this.productReviewService = productReviewService;
        }
        [AllowAnonymous]
        [HttpGet("prodcutReviews")]
        public ActionResult<List<ProductreviewDto>> GetAll()
        {
            var reviews = productReviewService.GetProductReviewDto();
            return Ok(reviews);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<ProductreviewDto> Get(int id)
        {
            var review = productReviewService.GetProductReviewDto(id);
            if (review == null)
                return NotFound();
            return Ok(review);
        }

        [HttpGet("byProduct/{productId}")]
        public ActionResult<List<ProductreviewDto>> GetReviewsByProductId(int productId)
        {
            var reviews = productReviewService.GetReviewsByProductId(productId);

            if (reviews == null || !reviews.Any())
                return NotFound();

            return Ok(reviews);
        }

        [HttpGet("byProductOfUser")]
        public ActionResult<ProductreviewDto> GetReviewsByProductIdOfUser(int productId, int userId)
        {
            var review = productReviewService.GetReviewsByProductIdOfUser(productId, userId);

            if (review == null)
                return Ok(null);

            return Ok(review);
        }

        [HttpPost("add")]
        public bool Add(ProductreviewDto dto)
        {
            bool result = productReviewService.AddProductreviewDto(dto);
            return result;
        }

        [HttpPost("edit")]
        public bool Update(ProductreviewDto dto)
        {
            var result = productReviewService.UpdateProductreviewDto(dto);
            return result;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = productReviewService.DeleteProductreviewDto(id);
            if (!result)
                return NotFound();
            return Ok();
        }
    }
}
