using Data;
using DataModel;
using Mapster;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class OrderdetailService : IOrdetailService
    {

        private readonly IDbContextFactory<DiceShopContext> diceShopContextFactory;
        public OrderdetailService(IDbContextFactory<DiceShopContext> contextFactory)
        {
            diceShopContextFactory = contextFactory;
        }

        public bool AddOrderDetail(OrderdetailDto orderdetailDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var order = diceShopContext.Orders.Find(orderdetailDto.OrderId);
            var orderdetailEntity = orderdetailDto.Adapt<Orderdetail>();
            orderdetailEntity.OrderId = order.Id;
            orderdetailEntity.Order = order;
            diceShopContext.Orderdetails.Add(orderdetailEntity);
            return diceShopContext.SaveChanges() > 0;           
        }

        public bool DeleteOrderDetail(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var orderDetail = diceShopContext.Orderdetails.Find(id);
            if (orderDetail == null) return false;

            diceShopContext.Orderdetails.Remove(orderDetail);
            return diceShopContext.SaveChanges() > 0;
        }

        public OrderdetailDto GetOrderDetail(int id)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            return diceShopContext.Orderdetails.FirstOrDefault(o => o.Id == id).Adapt<OrderdetailDto>();
        }

        public List<OrderdetailDto> GetOrderDetailByOrder(int orderId)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            return diceShopContext.Orderdetails.Where(o => o.OrderId == orderId).ProjectToType<OrderdetailDto>().ToList();
        }

        public List<OrderdetailDto> GetOrdersDteails()
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var OrderdetailsDto = diceShopContext.Orderdetails.ProjectToType<OrderdetailDto>().ToList();
            return OrderdetailsDto;
        }

        public bool UpdateOrderDetail(OrderdetailDto orderdetailDto)
        {
            using var diceShopContext = diceShopContextFactory.CreateDbContext();
            var orderdetail = diceShopContext.Orderdetails.FirstOrDefault(c => c.Id == orderdetailDto.Id);
            if (orderdetail == null) return false;
            orderdetailDto.Adapt(orderdetail);
            diceShopContext.Update(orderdetail);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}
