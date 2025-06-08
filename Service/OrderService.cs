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
    internal class OrderService : IOrderService
    {
        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public OrderService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public OrderDto AddOrder(OrderDto orderDto)
        {
            try
            {
                using var diceShopContext = diceShopContextFactory.CreateDbContext();
                var orderEntity = orderDto.Adapt<Order>();
                
                // Agregar correctamente la entidad al contexto
                diceShopContext.Orders.Add(orderEntity);

                diceShopContext.SaveChanges();
                var order = orderEntity;
                var dto = order.Adapt<OrderDto>();
                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el pedido: {ex.Message}");
                Console.WriteLine(ex.InnerException);
                return null;
            }

        }

        public bool DeleteOrder(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var order = diceShopContext.Orders.Find(id);
            if (order == null) return false;
            diceShopContext.Orders.Remove(order);
            return diceShopContext.SaveChanges() > 0;
        }

        public List<OrderDto> GetOrders()
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var ordersDto = diceShopContext.Orders.ProjectToType<OrderDto>().ToList();
            return ordersDto;
        }

        public List<OrderDto> GetOrdersByUser(int userId)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var ordersDto = diceShopContext.Orders.Where(o => o.UserId == userId).ProjectToType<OrderDto>().ToList();
            return ordersDto;
        }

        public OrderDto GetOrder(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            return diceShopContext.Orders.Find(id).Adapt<OrderDto>();
        }

        public bool UpdateOrder(OrderDto orderDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var order = diceShopContext.Orders.FirstOrDefault(c => c.Id == orderDto.Id);
            if (order == null) return false;
            orderDto.Adapt(order);
            diceShopContext.Update(order);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}
