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

        private DiceShopContext diceShopContext;
        public OrderdetailService(DiceShopContext diceShopContext)
        {
            this.diceShopContext = diceShopContext;
        }

        public bool AddOrderDetail(OrderdetailDto orderdetailDto)
        {
            var order = diceShopContext.Orders.Find(4);
            var orderdetailEntity = orderdetailDto.Adapt<Orderdetail>();
            orderdetailEntity.OrderId = order.Id;
            orderdetailEntity.Order = order;
            diceShopContext.Orderdetails.Add(orderdetailEntity);
            return diceShopContext.SaveChanges() > 0;           
        }

        public bool DeleteOrderDetail(int id)
        {
            var orderDetail = diceShopContext.Orderdetails.Find(id);
            if (orderDetail == null) return false;

            diceShopContext.Orderdetails.Remove(orderDetail);
            return diceShopContext.SaveChanges() > 0;
        }

        public OrderdetailDto GetOrderDetail(int id)
        {
            return diceShopContext.Orderdetails.Include(o => o.Product).FirstOrDefault(o => o.Id == id).Adapt<OrderdetailDto>();
        }
             
        public List<OrderdetailDto> GetOrdersDteails()
        {
            var OrderdetailsDto = diceShopContext.Orderdetails.ProjectToType<OrderdetailDto>().ToList();
            return OrderdetailsDto;
        }

        public bool UpdateOrderDetail(OrderdetailDto orderdetailDto)
        {
            var orderdetail = diceShopContext.Orderdetails.FirstOrDefault(c => c.Id == orderdetailDto.Id);
            if (orderdetail == null) return false;
            orderdetailDto.Adapt(orderdetail);
            diceShopContext.Update(orderdetail);
            return diceShopContext.SaveChanges() > 0;
        }
    }
}
