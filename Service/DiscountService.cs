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
        private DiceShopContext diceShopContext;
        public DiscountService(DiceShopContext diceShopContext)
        {                       
            this.diceShopContext = diceShopContext;
        }

        public bool AddDiscountDto(DiscountDto discountDto)
        {
            var discountEntity = discountDto.Adapt<Discount>();
            diceShopContext.Discounts.Add(discountEntity);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool DeleteDiscountDto(int id)
        {
            var discount = diceShopContext.Discounts.Find(id);
            if (discount == null) return false;

            diceShopContext.Discounts.Remove(discount);
            return diceShopContext.SaveChanges() > 0;
        }

        public List<DiscountDto> GetDiscountDto()
        {
            var discountDto = diceShopContext.Discounts.ProjectToType<DiscountDto>().ToList();
            return discountDto;
        }

        public DiscountDto GetDiscountDto(int id)
        {
            return diceShopContext.Discounts.Find(id).Adapt<DiscountDto>();
        }

        public async Task<PagedResult<DiscountDto>> GetDiscountsPagedAsync(int pageNumber, int pageSize, string? search)
        {
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
            var discount = diceShopContext.Discounts.FirstOrDefault(c => c.Id == discountDto.Id);
            if (discount == null) return false;
            discountDto.Adapt(discount);
            diceShopContext.Update(discount);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}
