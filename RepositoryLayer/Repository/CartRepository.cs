using ModelLayer;
using MongoDB.Driver;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<CartModel> Cart;
        
        public CartRepository(IDBSetting db)
        {
            var client = new MongoClient(db.ConnectionString);
            var database = client.GetDatabase(db.DatabaseName);
            Cart = database.GetCollection<CartModel>("Cart");
        }
        public async Task<CartModel> AddtoCart(CartModel cart)
        {
            try
            {
                var check = await this.Cart.Find(x => x.cartID == cart.cartID).SingleOrDefaultAsync();
                if(check == null)
                {
                    await this.Cart.InsertOneAsync(cart);
                    return cart;

                }
                return null;

            }
            catch(ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> RemovefromCart(CartModel del)
        {
            try
            {
                await this.Cart.FindOneAndDeleteAsync(x => x.cartID == del.cartID);
                return true;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CartModel> UpdateCartQty(CartModel qty)
        {
            try
            {
                var check = await this.Cart.Find(x => x.cartID == qty.cartID).FirstOrDefaultAsync();
                if (check != null)
                {
                    await this.Cart.UpdateOneAsync(x=>x.cartID== qty.cartID,
                        Builders<CartModel>.Update.Set(x=>x.quantity,qty.quantity));
                    return check;

                }
                return null;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<CartModel> GetCart()
        {
            return Cart.Find(FilterDefinition<CartModel>.Empty).ToList();

        }
    }
}
