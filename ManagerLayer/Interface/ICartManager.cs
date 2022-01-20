using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interface
{
    public interface ICartManager
    {
        Task<CartModel> AddtoCart(CartModel cart);
        Task<CartModel> UpdateCartQty(CartModel qty);
        Task<bool> RemovefromCart(CartModel del);
        IEnumerable<CartModel> GetCart();

    }
}
