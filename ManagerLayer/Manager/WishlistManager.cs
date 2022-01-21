using ManagerLayer.Interface;
using ModelLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Manager
{
    public class WishlistManager : IWishlistManager
    {
        private readonly IWishlistRepository repo;
        public WishlistManager(IWishlistRepository repo)
        {
            this.repo = repo;
        }
        public async Task<WishlistModel> AddToWishlist(WishlistModel wish)
        {
            try
            {
                return await this.repo.AddToWishlist(wish);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> RemoveWishlist(WishlistModel del)
        {
            try
            {
                return await this.repo.RemoveWishlist(del);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<WishlistModel> GetWishlist()
        {
            try
            {
                return this.repo.GetWishlist();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
