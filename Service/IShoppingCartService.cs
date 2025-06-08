using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IShoppingCartService
    {
        public bool AddShoppingcart(ShoppingcartDto dto);

        public ShoppingcartDto GetShoppingcarts(int id);
        public ShoppingcartDto GetShoppingcartActiveByUser(int userId);

        public List<ShoppingcartDto> GetShoppingcarts();

        public bool UpdateShoppingcart(ShoppingcartDto dto);

        public bool DeleteShoppingcart(int id);
    }
}
