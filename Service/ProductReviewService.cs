using Data;
using DataModel;
using Mapster;
using Microsoft.EntityFrameworkCore;
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
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public ProductReviewService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public bool AddProductreviewDto(ProductreviewDto productReviewDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var productReviewDtoEntity = productReviewDto.Adapt<Productreview>();
            diceShopContext.Productreviews.Add(productReviewDtoEntity);
            return diceShopContext.SaveChanges() > 0;
        }
        public List<ProductreviewDto> GetReviewsByProductId(int productId)
        {
            using var db = diceShopContextFactory.CreateDbContext();
            return db.Productreviews
                .Where(r => r.ProductId == productId)
                .ProjectToType<ProductreviewDto>() // Si usas Mapster
                .ToList();
        }

        public ProductreviewDto GetReviewsByProductIdOfUser(int productId, int userId)
        {
            using var db = diceShopContextFactory.CreateDbContext();
            return db.Productreviews
                .FirstOrDefault(r => r.ProductId == productId && r.UserId == userId)
                .Adapt<ProductreviewDto>(); // Si usas Mapster
        }

        public bool DeleteProductreviewDto(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var productReview = diceShopContext.Productreviews.Find(id);
            if (productReview == null) return false;

            diceShopContext.Productreviews.Remove(productReview);
            return diceShopContext.SaveChanges() > 0;
        }

        public List<ProductreviewDto> GetProductReviewDto()
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var productreviwDto = diceShopContext.Productreviews.ProjectToType<ProductreviewDto>().ToList();
            return productreviwDto;
        }

        public ProductreviewDto GetProductReviewDto(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            return diceShopContext.Productreviews.Find(id).Adapt<ProductreviewDto>();
        }

        public bool UpdateProductreviewDto(ProductreviewDto productreviewDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var productreview = diceShopContext.Productreviews.FirstOrDefault(c => c.Id == productreviewDto.Id);
            if (productreview == null) return false;
            productreviewDto.Adapt(productreview);
            diceShopContext.Update(productreview);
            return diceShopContext.SaveChanges() > 0;
        }

    }
}
