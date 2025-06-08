using Data;
using DataModel;
using Mapster;
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
        private DiceShopContext diceShopContext;
        public OrderService(DiceShopContext diceShopContext)
        {
            this.diceShopContext = diceShopContext;
        }

        public bool AddOrder(OrderDto orderDto)
        {
            try
            {
                var orderEntity = orderDto.Adapt<Order>();
                
                // Agregar correctamente la entidad al contexto
                diceShopContext.Orders.Add(orderEntity);

                return diceShopContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el pedido: {ex.Message}");
                Console.WriteLine(ex.InnerException);
                return false;
            }

        }

        public bool DeleteOrder(int id)
        {
            var order = diceShopContext.Orders.Find(id);
            if (order == null) return false;
            diceShopContext.Orders.Remove(order);
            return diceShopContext.SaveChanges() > 0;
        }

        public List<OrderDto> GetOrders()
        {
            var ordersDto = diceShopContext.Orders.ProjectToType<OrderDto>().ToList();
            return ordersDto;
        }

        public OrderDto GetOrder(int id)
        {
            return diceShopContext.Orders.Find(id).Adapt<OrderDto>();
        }

        public bool UpdateOrder(OrderDto orderDto)
        {
            var order = diceShopContext.Orders.FirstOrDefault(c => c.Id == orderDto.Id);
            if (order == null) return false;
            orderDto.Adapt(order);
            diceShopContext.Update(order);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}
