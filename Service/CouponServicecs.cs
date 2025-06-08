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
    internal class CouponService : ICouponService
    {
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public CouponService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public List<CouponDto> GetCoupons()
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            return diceShopContext.Coupons
                .AsNoTracking()
                .ProjectToType<CouponDto>()
                .ToList();
        }

        public async Task<PagedResult<CouponDto>> GetCouponsPagedAsync(int pageNumber, int pageSize, string? search)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var query = diceShopContext.Coupons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(d => d.Code.Contains(search));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(d => d.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<CouponDto>()
                .ToListAsync();

            return new PagedResult<CouponDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public CouponDto GetCouponById(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var coupon = diceShopContext.Coupons.Find(id);
            return coupon?.Adapt<CouponDto>();
        }

        public bool AddCoupon(CouponDto couponDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var entity = couponDto.Adapt<Coupon>();
            diceShopContext.Coupons.Add(entity);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool UpdateCoupon(CouponDto couponDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var coupon = diceShopContext.Coupons.FirstOrDefault(c => c.Code == couponDto.Code);
            if (coupon == null) return false;

            couponDto.Adapt(coupon);
            diceShopContext.Coupons.Update(coupon);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool DeleteCoupon(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var coupon = diceShopContext.Coupons.Find(id);
            if (coupon == null) return false;

            diceShopContext.Coupons.Remove(coupon);
            return diceShopContext.SaveChanges() > 0;
        }
        public bool UseCoupon(int couponId)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var coupon = diceShopContext.Coupons.FirstOrDefault(c => c.Id == couponId);
            if (coupon == null || coupon.UsedCount >= coupon.Quantity) return false;

            coupon.UsedCount++;
            diceShopContext.Update(coupon);
            return diceShopContext.SaveChanges() > 0;
        }

        public CouponDto GetCouponByCode(string code)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var coupon = diceShopContext.Coupons.FirstOrDefault(c => c.Code == code);
            return coupon?.Adapt<CouponDto>();
        }

    }

}



