using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistManager manager;

        [HttpPost]
        [Route("addtowishlist")]
        public async Task<IActionResult> AddToWishlist(WishlistModel wish)
        {
            try
            {
                var check = await this.manager.AddToWishlist(wish);
                if (check != null)
                {
                    return this.Ok(new ResponseModel<WishlistModel> { Status = true, Message = "Book Added to Wishlist", Data = check });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Book Not Added to Wishlist" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpDelete]
        [Route("removefromwishlist")]
        public async Task<IActionResult> RemoveWishlist(WishlistModel del)
        {
            try
            {
                var check = await this.manager.RemoveWishlist(del);
                if (check != false)
                {
                    return this.Ok(new ResponseModel<WishlistModel> { Status = true, Message = "Book Removed from Wishlist" });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Book not Removed from Wishlist" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpGet]
        [Route("getwishlist")]
        public IActionResult GetWishlist()
        {
            try
            {
                IEnumerable<WishlistModel> check = this.manager.GetWishlist();
                if (check != null)
                {
                    return this.Ok(new { Status = true, Message = "Wishlist Retrived Successfully", Data = check });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Wishlist is Empty" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }
    }
}
