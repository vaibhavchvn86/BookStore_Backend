using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interface
{
    public interface IWishlistManager
    {
        Task<WishlistModel> AddToWishlist(WishlistModel wish);
        Task<bool> RemoveWishlist(WishlistModel del);
        IEnumerable<WishlistModel> GetWishlist();
    }
}
