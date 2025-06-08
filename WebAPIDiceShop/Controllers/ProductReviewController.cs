using DataModel;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductReviewController
    {
        private IProductReviewService productReviewService;

        public ProductReviewController(IProductReviewService productReviewService)
        {
            this.productReviewService = productReviewService;
        }

        [HttpGet("  ")]
        public List<ProductreviewDto> GetProductreviewDto()
        {
            return productReviewService.GetProductReviewDto();
        }

        [HttpPost("add")]
        public bool PosProductReview(ProductreviewDto productreviewDto)
        {

            var createrProductreview = productReviewService.AddProductreviewDto(productreviewDto);
            return createrProductreview;
        }

        [HttpPut("edit")]
        public bool EditProductReview(ProductreviewDto productreviewDto)
        {
            return productReviewService.UpdateProductreviewDto(productreviewDto);
        }

        [HttpDelete("delete/{id}")]
        public bool DeleteDiscount(int id)
        {
            return productReviewService.DeleteProductreviewDto(id);
        }
    }
}
