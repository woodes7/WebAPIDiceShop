using Microsoft.AspNetCore.Mvc;
using DataModel;
using Service;
using Model;

namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BillingAddressController
    {

        private IBillingAddressService billingAddressService;

        public BillingAddressController(IBillingAddressService billingAddressService)
        {
            this.billingAddressService = billingAddressService;
        }

        [HttpGet("billingAdresses")]
        public List<BillingaddressDto> GetBillingaddresses()
        {
            return this.billingAddressService.GetBillingAddressesDto();

        }

        [HttpGet("billingAdressesPaged")]
        public async Task<PagedResult<BillingaddressDto>> GetPagedBillingAddresses(int pageNumber, int pageSize, string? search = null)
        {
            return await billingAddressService.GetPagedBillingAddressesAsync(pageNumber, pageSize, search);
        }

        [HttpGet("billingAddress/{id}")]
        public BillingaddressDto GetBillingaddress(int id)
        {
            return this.billingAddressService.GetBillingAddressDto(id);
        }

        [HttpPost("add")]
        public bool PostBillingAddress(BillingaddressDto billingaddressDto)
        {
            var createdBillingAdress = billingAddressService.AddBillingAddressDto(billingaddressDto);
            return createdBillingAdress;
        }

        [HttpPut("edit")]
        public bool PutBillingAddress(BillingaddressDto billingaddressDto)
        {
            return this.billingAddressService.UpdateBillingAddressDto(billingaddressDto);
        }

        [HttpDelete("delete")]
        public bool DeleteBillingAddress(int id)
        {
            return billingAddressService.DeleteBillingAddressDto(id);
        }

        [HttpGet("primary/{userId}")]
        public BillingaddressDto? GetPrimaryAddress(int userId)
        {
            return this.billingAddressService.GetPrimaryAddressByUserId(userId);
        }

    }
}


    

    