using ModelLayer;
using MongoDB.Driver;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly IMongoCollection<WishlistModel> Wishlist;

        public WishlistRepository(IDBSetting db)
        {
            var client = new MongoClient(db.ConnectionString);
            var database = client.GetDatabase(db.DatabaseName);
            Wishlist = database.GetCollection<WishlistModel>("Wishlist");
        }
        public async Task<WishlistModel> AddToWishlist(WishlistModel wish)
        {
            try
            {
                var check = await this.Wishlist.Find(x => x.wishlistID == wish.wishlistID).SingleOrDefaultAsync();
                if (check == null)
                {
                    await this.Wishlist.InsertOneAsync(wish);
                    return wish;

                }
                return null;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> RemoveWishlist(WishlistModel del)
        {
            try
            {
                await this.Wishlist.FindOneAndDeleteAsync(x => x.wishlistID == del.wishlistID);
                return true;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<WishlistModel> GetWishlist()
        {
            return Wishlist.Find(FilterDefinition<WishlistModel>.Empty).ToList();
        }
    }
}
