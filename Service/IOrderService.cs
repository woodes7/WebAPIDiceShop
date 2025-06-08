using DataModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IOrderService
    {
        public List<OrderDto> GetOrders();

        public OrderDto GetOrder(int id);

        public bool AddOrder(OrderDto orderDto);

        public bool UpdateOrder(OrderDto orderDto);

        public bool DeleteOrder(int id);
    }
}
