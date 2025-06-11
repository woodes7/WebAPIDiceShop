using DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPIDiceShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController
    {
        private IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;        
        }

        [HttpGet("order")]
        public List<OrderDto> GetOrders()
        {
            return this.orderService.GetOrders();
        }

        [HttpGet("orderByUser")]
        public List<OrderDto> GetOrders(int userId)
        {
            return this.orderService.GetOrdersByUser(userId);
        }

        [HttpGet("order/{id}")]
        public OrderDto GetOrder(int id)
        {
            return this.orderService.GetOrder(id);
        }

        [HttpPost("add")]
        public OrderDto UpdateOrder(OrderDto order)
        {
            var createOrder = orderService.AddOrder(order);
            return createOrder;
        }

        [HttpPut("edit")]
        public bool EditOrder(OrderDto order)
        {
            return this.orderService.UpdateOrder(order);
        }

        [HttpDelete("delete/{id}")]
        public bool DeleteOrder(int id)
        {
            return this.orderService.DeleteOrder(id);
        }
    }
}
