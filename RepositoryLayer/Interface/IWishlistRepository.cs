using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IWishlistRepository
    {
        Task<WishlistModel> AddToWishlist(WishlistModel wish);
        Task<bool> RemoveWishlist(WishlistModel del);
        IEnumerable<WishlistModel> GetWishlist();

    }
}
