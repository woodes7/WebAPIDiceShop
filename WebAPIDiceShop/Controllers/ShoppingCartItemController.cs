using DataModel;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace WebAPIDiceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartItemController : ControllerBase
    {
        private readonly IShoppingCartItemService itemService;

        public ShoppingCartItemController(IShoppingCartItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpGet("{cartId}/items")]
        public List<ShoppingcartitemDto> GetItemsByCartId(int cartId, bool isCart = false)
        {
            return itemService.GetItemsByCartId(cartId, isCart);
        }

        [HttpGet("item/{id}")]
        public ShoppingcartitemDto GetItemById(int id)
        {
            return itemService.GetItemDto(id);
        }

        [HttpPost("add")]
        public bool AddItem(ShoppingcartitemDto dto)
        {
            return itemService.AddItemDto(dto);
        }

        [HttpPut("edit")]
        public bool EditItem(ShoppingcartitemDto dto)
        {
            return itemService.UpdateItemDto(dto);
        }

        [HttpDelete("delete/{id}")]
        public bool DeleteItem(int id)
        {
            return itemService.DeleteItemDto(id);
        }
    }
}