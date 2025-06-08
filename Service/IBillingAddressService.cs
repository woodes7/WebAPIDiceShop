using DataModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBillingAddressService
    {
        public List<BillingaddressDto> GetBillingAddressesDto();
        public List<BillingaddressDto> GetBillingAddressesByUser(int userId);
        public BillingaddressDto GetPrimaryBillingaddress(int userId);
        public Task<PagedResult<BillingaddressDto>> GetPagedBillingAddressesAsync(int pageNumber, int pageSize, string? search = null);

        public BillingaddressDto GetBillingAddressDto(int id);

        public bool AddBillingAddressDto(BillingaddressDto billingAddressDto);

        public bool UpdateBillingAddressDto(BillingaddressDto billingAddressDto);

        public bool DeleteBillingAddressDto(int id);

        BillingaddressDto? GetPrimaryAddressByUserId(int userId);


    }
}
