using Data;
using Mapster;
using DataModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



namespace Service
{
    public class BillingAddressService : IBillingAddressService
    {
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public BillingAddressService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public bool AddBillingAddressDto(BillingaddressDto billingAddressDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            // Si se marca como principal, desactivar otras del mismo usuario
            if (billingAddressDto.IsPrimary == true)
            {
                var otherPrimaries = diceShopContext.Billingaddresses
                    .Where(a => a.UserId == billingAddressDto.UserId && a.IsPrimary == true);

                foreach (var addr in otherPrimaries)
                {
                    addr.IsPrimary = false;
                }
            }

            var billingAddressDtoEntity = billingAddressDto.Adapt<Billingaddress>();
            billingAddressDtoEntity.CreationDate = DateTime.Now;

            diceShopContext.Billingaddresses.Add(billingAddressDtoEntity);
            return diceShopContext.SaveChanges() > 0;
        }


        public bool DeleteBillingAddressDto(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var billingAdress = diceShopContext.Billingaddresses.Find(id);
            if (billingAdress == null) return false;

            diceShopContext.Billingaddresses.Remove(billingAdress);
            return diceShopContext.SaveChanges() > 0;
        }

        public List<BillingaddressDto> GetBillingAddressesDto()
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var billingAdressesDto = diceShopContext.Billingaddresses.ProjectToType<BillingaddressDto>().ToList();
            return billingAdressesDto;
        }

        public List<BillingaddressDto> GetBillingAddressesByUser(int userId)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var billingAdressesDto = diceShopContext.Billingaddresses.Where(b => b.UserId == userId).ProjectToType<BillingaddressDto>().ToList();
            return billingAdressesDto;
        }

        public async Task<PagedResult<BillingaddressDto>> GetPagedBillingAddressesAsync(int pageNumber, int pageSize, string? search = null)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var query = diceShopContext.Billingaddresses.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Street.Contains(search) || b.Country.Contains(search) || b.City.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<BillingaddressDto>()
                .ToListAsync();

            return new PagedResult<BillingaddressDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public BillingaddressDto GetPrimaryBillingaddress(int userId)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            return diceShopContext.Billingaddresses.FirstOrDefault(b => b.UserId == userId).Adapt<BillingaddressDto>();
        }

        public BillingaddressDto GetBillingAddressDto(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var billingAddress = diceShopContext.Billingaddresses.Include(b => b.User).FirstOrDefault(b => b.Id == id);
            var billingAddressDto = billingAddress.Adapt<BillingaddressDto>();
            return billingAddressDto;
        }

        public bool UpdateBillingAddressDto(BillingaddressDto billingAddressDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var billingAddress = diceShopContext.Billingaddresses
                .FirstOrDefault(c => c.Id == billingAddressDto.Id);
            if (billingAddress == null) return false;

            // Si la nueva dirección está marcada como principal
            if (billingAddressDto.IsPrimary == true)
            {
                // Buscar otras direcciones principales del mismo usuario (excepto esta) y desmarcarlas
                var otherPrimaries = diceShopContext.Billingaddresses
                    .Where(a => a.UserId == billingAddressDto.UserId && a.Id != billingAddressDto.Id && a.IsPrimary == true);

                foreach (var addr in otherPrimaries)
                {
                    addr.IsPrimary = false;
                }
            }

            // Mapear el dto sobre la entidad existente
            billingAddressDto.Adapt(billingAddress);

            diceShopContext.Update(billingAddress);
            return diceShopContext.SaveChanges() > 0;
        }


        public BillingaddressDto GetPrimaryAddressByUserId(int userId)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var address = diceShopContext.Billingaddresses
                .FirstOrDefault(a => a.UserId == userId && a.IsPrimary == true);

            if (address == null) return null;

            return address.Adapt<BillingaddressDto>();
        }

    }
}
