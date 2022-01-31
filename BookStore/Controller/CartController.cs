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
    public class CartController : ControllerBase
    {
        private readonly ICartManager manager;

        public CartController(ICartManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("addtocart")]
        public async Task<IActionResult> AddtoCart([FromBody] CartModel cart)
        {
            try
            {
                var check = await this.manager.AddtoCart(cart);
                if(check != null)
                {
                    return this.Ok(new ResponseModel<CartModel> { Status = true, Message = "Book Added to Cart", Data = check });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Book Not Added to Cart"});
                }
            }
            catch(Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message});
            }
        }

        [HttpPut]
        [Route("updatequantity")]
        public async Task<IActionResult> UpdateCartQty([FromBody] CartModel edit)
        {
            try
            {
                var check = await this.manager.UpdateCartQty(edit);
                if (check != null)
                {
                    return this.Ok(new ResponseModel<CartModel> { Status = true, Message = "Book Quantity Updated", Data = check });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Cannot Update Quantity" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpDelete]
        [Route("deletefromcart")]
        public async Task<IActionResult> RemovefromCart([FromBody] CartModel del)
        {
            try
            {
                var check = await this.manager.RemovefromCart(del);
                if (check != false)
                {
                    return this.Ok(new ResponseModel<CartModel> { Status = true, Message = "Book Removed from Cart"});
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Book not Removed from Cart" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpGet]
        [Route("getcart")]
        public IActionResult GetCart()
        {
            try
            {
                IEnumerable<CartModel> check = this.manager.GetCart();
                if (check != null)
                {
                    return this.Ok(new { Status = true, Message = "Cart Retrived Successfully", Data = check });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Cart is Empty" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }
    }
}
