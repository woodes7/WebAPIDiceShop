using DataModel;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPIDiceShop.Controllers
{
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

        [HttpGet("order/{id}")]
        public OrderDto GetOrder(int id)
        {
            return this.orderService.GetOrder(id);
        }

        [HttpPost("add")]
        public bool UpdateOrder(OrderDto order)
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
