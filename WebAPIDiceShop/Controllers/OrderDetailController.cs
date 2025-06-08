using DataModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;


namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OrderDetailController
    {
        private IOrdetailService ordetailService;

        public OrderDetailController(IOrdetailService ordetailService)
        {
            this.ordetailService = ordetailService;
        }

        [HttpGet("orderDetail")]
        public List<OrderdetailDto> GetOrders()
        {
            return this.ordetailService.GetOrdersDteails();
        }
       
        [HttpGet("orderDetail/{id}")]
        public OrderdetailDto GetOrderDetail(int id)
        {
            return this.ordetailService.GetOrderDetail(id);
        }

        [HttpPost("add")]
        public bool PostUser(OrderdetailDto orderDetailDto)
        {
            var createOrderDte = ordetailService.AddOrderDetail(orderDetailDto);
            return createOrderDte;
        }

        [HttpPut("edit")]
        public bool PutOrderDetail(OrderdetailDto orderDetailDto)
        {
            return ordetailService.UpdateOrderDetail(orderDetailDto);
        }

        [HttpDelete("delete/{id}")]
        public bool DeleteOrderDetail(int id)
        {
            return ordetailService.DeleteOrderDetail(id);
        }
    }
}
