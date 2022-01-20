using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ICartRepository
    {
        Task<CartModel> AddtoCart(CartModel cart);
        Task<CartModel> UpdateCartQty(CartModel qty);
        Task<bool> RemovefromCart(CartModel del);
        IEnumerable<CartModel> GetCart();
    }
}
