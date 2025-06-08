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
    internal class DiscountService : IDiscountService
    {
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public DiscountService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public bool AddDiscountDto(DiscountDto discountDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var discountEntity = discountDto.Adapt<Discount>();
            diceShopContext.Discounts.Add(discountEntity);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool DeleteDiscountDto(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var discount = diceShopContext.Discounts.Find(id);
            if (discount == null) return false;

            diceShopContext.Discounts.Remove(discount);
            return diceShopContext.SaveChanges() > 0;
        }

        public List<DiscountDto> GetDiscountDto()
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var discountDto = diceShopContext.Discounts.ProjectToType<DiscountDto>().ToList();
            return discountDto;
        }

        public DiscountDto GetDiscountDto(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            return diceShopContext.Discounts.Find(id).Adapt<DiscountDto>();
        }

        public async Task<PagedResult<DiscountDto>> GetDiscountsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var query = diceShopContext.Discounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(d => d.Code.Contains(search));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(d => d.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<DiscountDto>()
                .ToListAsync();

            return new PagedResult<DiscountDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }


        public bool UpdateDiscountDto(DiscountDto discountDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var discount = diceShopContext.Discounts.FirstOrDefault(c => c.Id == discountDto.Id);
            if (discount == null) return false;
            discountDto.Adapt(discount);
            diceShopContext.Update(discount);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}
