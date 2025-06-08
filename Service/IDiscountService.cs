using DataModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IDiscountService
    {
        public List<DiscountDto> GetDiscountDto();

        public Task<PagedResult<DiscountDto>> GetDiscountsPagedAsync(int pageNumber, int pageSize, string? search);

        public DiscountDto GetDiscountDto(int id);

        public bool AddDiscountDto(DiscountDto discountDto);

        public bool UpdateDiscountDto(DiscountDto discountDto);

        public bool DeleteDiscountDto(int id);
    }
}
