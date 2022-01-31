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
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager manager;
        public OrderController(IOrderManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("addorder")]
        public async Task<IActionResult> AddOrder([FromBody] OrderModel order)
        {
            try
            {
                var check = await this.manager.AddOrder(order);
                if (check != null)
                {
                    return this.Ok(new ResponseModel<OrderModel> { Status = true, Message = "Order Placed", Data = check });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Order not Placed" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpDelete]
        [Route("cancleorder")]
        public async Task<IActionResult> CancleOrder([FromBody] OrderModel del)
        {
            try
            {
                var check = await this.manager.CancleOrder(del);
                if (check != false)
                {
                    return this.Ok(new ResponseModel<OrderModel> { Status = true, Message = "Order Canclled" });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Order not Canclled" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpGet]
        [Route("getorder")]
        public IActionResult GetOrder()
        {
            try
            {
                IEnumerable<OrderModel> check = this.manager.GetOrder();
                if (check != null)
                {
                    return this.Ok(new { Status = true, Message = "Order Retrived Successfully", Data = check });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Order is Empty" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }
    }
}
