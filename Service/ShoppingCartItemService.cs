using Data;
using DataModel;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    internal class ShoppingCartItemService : IShoppingCartItemService
    {
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public ShoppingCartItemService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public bool AddItemDto(ShoppingcartitemDto dto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            dto.AddedAt = DateTime.Now;
            var entity = dto.Adapt<Shoppingcartitem>();
            diceShopContext.Shoppingcartitems.Add(entity);
            return diceShopContext.SaveChanges() > 0;
        }

        public ShoppingcartitemDto GetItemDto(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var entity = diceShopContext.Shoppingcartitems
                .Include(i => i.Product)
                .FirstOrDefault(i => i.Id == id);

            if (entity == null) return null;

            var dto = entity.Adapt<ShoppingcartitemDto>();
            return dto;
        }

        public List<ShoppingcartitemDto> GetItemsByCartId(int cartId, bool onlyActive = false)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var query = diceShopContext.Shoppingcartitems
                .Include(i => i.Product)
                .Where(i => i.ShoppingCartId == cartId);

            if (onlyActive)
            {
                query = query.Where(i => i.Active == true);
            }

           return query.ProjectToType<ShoppingcartitemDto>().ToList();

        }


        public bool UpdateItemDto(ShoppingcartitemDto dto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var entity = diceShopContext.Shoppingcartitems.FirstOrDefault(i => i.Id == dto.Id);
            if (entity == null) return false;

            dto.Adapt(entity);
            diceShopContext.Shoppingcartitems.Update(entity);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool DeleteItemDto(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var entity = diceShopContext.Shoppingcartitems.Find(id);
            if (entity == null) return false;

            diceShopContext.Shoppingcartitems.Remove(entity);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}