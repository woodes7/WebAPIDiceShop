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
        private readonly DiceShopContext context;

        public ShoppingCartItemService(DiceShopContext context)
        {
            this.context = context;
        }

        public bool AddItemDto(ShoppingcartitemDto dto)
        {
            dto.AddedAt = DateTime.Now;
            var entity = dto.Adapt<Shoppingcartitem>();
            context.Shoppingcartitems.Add(entity);
            return context.SaveChanges() > 0;
        }

        public ShoppingcartitemDto GetItemDto(int id)
        {
            var entity = context.Shoppingcartitems
                .Include(i => i.Product)
                .FirstOrDefault(i => i.Id == id);

            if (entity == null) return null;

            var dto = entity.Adapt<ShoppingcartitemDto>();
            return dto;
        }

        public List<ShoppingcartitemDto> GetItemsByCartId(int cartId, bool onlyActive = false)
        {
            var query = context.Shoppingcartitems
                .Include(i => i.Product)
                .Where(i => i.ShoppingCartId == cartId);

            if (onlyActive)
            {
                query = query.Where(i => i.Active == true);
            }

            var items = query.ToList();

            return items.Select(i => i.Adapt<ShoppingcartitemDto>()).ToList();
        }


        public bool UpdateItemDto(ShoppingcartitemDto dto)
        {
            var entity = context.Shoppingcartitems.FirstOrDefault(i => i.Id == dto.Id);
            if (entity == null) return false;

            dto.Adapt(entity);
            context.Shoppingcartitems.Update(entity);
            return context.SaveChanges() > 0;
        }

        public bool DeleteItemDto(int id)
        {
            var entity = context.Shoppingcartitems.Find(id);
            if (entity == null) return false;

            context.Shoppingcartitems.Remove(entity);
            return context.SaveChanges() > 0;
        }
    }
}