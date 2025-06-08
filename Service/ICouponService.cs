using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using Model;

namespace Service
{
    public interface ICouponService
    {
        public List<CouponDto> GetCoupons();
        public Task<PagedResult<CouponDto>> GetCouponsPagedAsync(int pageNumber, int pageSize, string? search);
        public CouponDto GetCouponById(int id);
        public bool AddCoupon(CouponDto couponDto);
        public bool UpdateCoupon(CouponDto couponDto);
        public bool DeleteCoupon(int id);
        public bool UseCoupon(int couponId);
        public CouponDto GetCouponByCode(string code);

    }
}
