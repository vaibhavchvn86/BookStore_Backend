using ManagerLayer.Interface;
using ModelLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Manager
{
    public class CartManager : ICartManager
    {
        private readonly ICartRepository repo;
        public CartManager(ICartRepository repo)
        {
            this.repo = repo;
        }
        public async Task<CartModel> AddtoCart(CartModel cart)
        {
            try
            {
                return await this.repo.AddtoCart(cart);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        public async Task<bool> RemovefromCart(CartModel del)
        {
            try
            {
                return await this.repo.RemovefromCart(del);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CartModel> UpdateCartQty(CartModel qty)
        {
            try
            {
                return await this.repo.UpdateCartQty(qty);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<CartModel> GetCart()
        {
            try
            {
                return this.repo.GetCart();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
