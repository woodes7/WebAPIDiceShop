using Data;
using DataModel;
using Mapster;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class ProductReviewService : IProductReviewService
    {
        private DiceShopContext diceShopContext;
        public ProductReviewService(DiceShopContext diceShopContext)
        {
            this.diceShopContext = diceShopContext;
        }

        public bool AddProductreviewDto(ProductreviewDto productReviewDto)
        {
            var productReviewDtoEntity = productReviewDto.Adapt<Productreview>();
            diceShopContext.Productreviews.Add(productReviewDtoEntity);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool DeleteProductreviewDto(int id)
        {
            var productReview = diceShopContext.Productreviews.Find(id);
            if (productReview == null) return false;

            diceShopContext.Productreviews.Remove(productReview);
            return diceShopContext.SaveChanges() > 0;
        }

        public List<ProductreviewDto> GetProductReviewDto()
        {
            var productreviwDto = diceShopContext.Productreviews.ProjectToType<ProductreviewDto>().ToList();
            return productreviwDto;
        }

        public ProductreviewDto GetProductReviewDto(int id)
        {
            return diceShopContext.Productreviews.Find(id).Adapt<ProductreviewDto>();
        }

        public bool UpdateProductreviewDto(ProductreviewDto productreviewDto)
        {
            var productreview = diceShopContext.Productreviews.FirstOrDefault(c => c.Id == productreviewDto.Id);
            if (productreview == null) return false;
            productreviewDto.Adapt(productreview);
            diceShopContext.Update(productreview);
            return diceShopContext.SaveChanges() > 0;
        }

    }
}
