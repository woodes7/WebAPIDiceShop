using Data;
using DataModel;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    internal class ShoppingCartService : IShoppingCartService
    {
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public ShoppingCartService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public bool AddShoppingcart(ShoppingcartDto dto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            dto.AddedDate = DateTime.Now;
            var entity = dto.Adapt<Shoppingcart>();
            diceShopContext.Shoppingcarts.Add(entity);
            return diceShopContext.SaveChanges() > 0;
        }

        public ShoppingcartDto GetShoppingcarts(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var entity = diceShopContext.Shoppingcarts
                .Include(c => c.User)
                .FirstOrDefault(c => c.Id == id);

            if (entity == null) return null;

            var dto = entity.Adapt<ShoppingcartDto>();
            return dto;
        }

        public ShoppingcartDto GetShoppingcartActiveByUser(int userId)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var entity = diceShopContext.Shoppingcarts
                .Include(c => c.User)
                .FirstOrDefault(c => c.UserId == userId);

            if (entity == null) return null;

            var dto = entity.Adapt<ShoppingcartDto>();
            return dto;
        }

        public List<ShoppingcartDto> GetShoppingcarts()
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var carts = diceShopContext.Shoppingcarts.Include(c => c.User).ToList();
            return carts.Select(c =>
            {
                var dto = c.Adapt<ShoppingcartDto>();
                return dto;
            }).ToList();
        }

        public bool UpdateShoppingcart(ShoppingcartDto dto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var entity = diceShopContext.Shoppingcarts.FirstOrDefault(c => c.Id == dto.Id);
            if (entity == null) return false;

            dto.Adapt(entity);
            diceShopContext.Shoppingcarts.Update(entity);
            return diceShopContext.SaveChanges() > 0;
        }

        public bool DeleteShoppingcart(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var entity = diceShopContext.Shoppingcarts.Find(id);
            if (entity == null) return false;

            diceShopContext.Shoppingcarts.Remove(entity);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}