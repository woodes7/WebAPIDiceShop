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
        private readonly DiceShopContext context;

        public ShoppingCartService(DiceShopContext context)
        {
            this.context = context;
        }

        public bool AddShoppingcart(ShoppingcartDto dto)
        {
            dto.AddedDate = DateTime.Now;
            dto.Active = true;
            var entity = dto.Adapt<Shoppingcart>();
            context.Shoppingcarts.Add(entity);
            return context.SaveChanges() > 0;
        }

        public ShoppingcartDto GetShoppingcarts(int id)
        {
            var entity = context.Shoppingcarts
                .Include(c => c.User)
                .FirstOrDefault(c => c.Id == id);

            if (entity == null) return null;

            var dto = entity.Adapt<ShoppingcartDto>();
            return dto;
        }

        public ShoppingcartDto GetShoppingcartActiveByUser(int userId)
        {
            var entity = context.Shoppingcarts
                .Include(c => c.User)
                .FirstOrDefault(c => c.UserId == userId && c.Active);

            if (entity == null) return null;

            var dto = entity.Adapt<ShoppingcartDto>();
            return dto;
        }

        public List<ShoppingcartDto> GetShoppingcarts()
        {
            var carts = context.Shoppingcarts.Include(c => c.User).ToList();
            return carts.Select(c =>
            {
                var dto = c.Adapt<ShoppingcartDto>();
                return dto;
            }).ToList();
        }

        public bool UpdateShoppingcart(ShoppingcartDto dto)
        {
            var entity = context.Shoppingcarts.FirstOrDefault(c => c.Id == dto.Id);
            if (entity == null) return false;

            dto.Adapt(entity);
            context.Shoppingcarts.Update(entity);
            return context.SaveChanges() > 0;
        }

        public bool DeleteShoppingcart(int id)
        {
            var entity = context.Shoppingcarts.Find(id);
            if (entity == null) return false;

            context.Shoppingcarts.Remove(entity);
            return context.SaveChanges() > 0;
        }
    }
}