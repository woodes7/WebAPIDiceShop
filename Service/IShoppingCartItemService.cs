using DataModel;
using Model;

namespace Service
{
    public interface IShoppingCartItemService
    {
        public bool AddItemDto(ShoppingcartitemDto dto);

        public ShoppingcartitemDto GetItemDto(int id);

        public List<ShoppingcartitemDto> GetItemsByCartId(int cartId,bool isCart = false);

        public bool UpdateItemDto(ShoppingcartitemDto dto);

        public bool DeleteItemDto(int id);
    }
}