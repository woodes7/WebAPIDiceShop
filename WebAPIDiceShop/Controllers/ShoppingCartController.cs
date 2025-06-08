using DataModel;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public List<ShoppingcartDto> GetShoppingCarts()
        {
            return shoppingCartService.GetShoppingcarts();
        }

        [HttpGet("{id}")]
        public ShoppingcartDto GetShoppingCartById(int id)
        {
            return shoppingCartService.GetShoppingcarts(id);
        }

        [HttpGet("getByUser")]
        public ShoppingcartDto GetShoppingCartByUserId(int userId)
        {
            return shoppingCartService.GetShoppingcartActiveByUser(userId);
        }

        [HttpPost("add")]
        public bool AddShoppingCart(ShoppingcartDto dto)
        {
            return shoppingCartService.AddShoppingcart(dto);
        }

        [HttpPut("edit")]
        public bool EditShoppingCart(ShoppingcartDto dto)
        {
            return shoppingCartService.UpdateShoppingcart(dto);
        }

        [HttpDelete("delete/{id}")]
        public bool DeleteShoppingCart(int id)
        {
            return shoppingCartService.DeleteShoppingcart(id);
        }
    }
}