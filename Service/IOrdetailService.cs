using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;



namespace Service
{
    public interface IOrdetailService
    {
        public List<OrderdetailDto> GetOrdersDteails();
        public List<OrderdetailDto> GetOrderDetailByOrder(int orderId);
        public OrderdetailDto GetOrderDetail(int id);

        public bool AddOrderDetail(OrderdetailDto orderdetailDto);

        public bool UpdateOrderDetail(OrderdetailDto orderdetailDto);

        public bool DeleteOrderDetail(int id);

    }
}
