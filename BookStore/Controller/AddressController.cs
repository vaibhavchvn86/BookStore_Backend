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
    public class AddressController : ControllerBase
    {
        private readonly IAddressManager manager;

        public AddressController(IAddressManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("addaddress")]
        public async Task<IActionResult> AddAddress([FromBody] AddressModel add)
        {
            try
            {
                var check = await this.manager.AddAddress(add);
                if(check != null)
                {
                    return this.Ok(new ResponseModel<AddressModel> { Status = true, Message = "Address Added Successfully", Data = check });
                }
                else
                {
                    return this.Ok(new { Status = false, Message = "Address not Added"});
                }
            }
            catch(Exception e)
            {
                return this.Ok(new { Status = false, Message = e.Message});
            }
        }

        [HttpPut]
        [Route("updateaddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressModel edit)
        {
            try
            {
                var check = await this.manager.AddAddress(edit);
                if (check != null)
                {
                    return this.Ok(new ResponseModel<AddressModel> { Status = true, Message = "Address Updated Successfully", Data = check });
                }
                else
                {
                    return this.Ok(new { Status = false, Message = "Address not Updated" });
                }
            }
            catch (Exception e)
            {
                return this.Ok(new { Status = false, Message = e.Message });
            }
        }

        [HttpDelete]
        [Route("deleteaddress")]
        public async Task<IActionResult> DeleteAddress([FromBody] AddressModel del)
        {
            try
            {
                var check = await this.manager.DeleteAddress(del);
                if (check == true)
                {
                    return this.Ok(new ResponseModel<AddressModel> { Status = true, Message = "Address Deleted Successfully" });
                }
                else
                {
                    return this.Ok(new { Status = false, Message = "Address not Deleted" });
                }
            }
            catch (Exception e)
            {
                return this.Ok(new { Status = false, Message = e.Message });
            }
        }

        [HttpGet]
        [Route("getalladdress")]
        public IActionResult GetallAddress()
        {
            try
            {
                IEnumerable<AddressModel> check = this.manager.GetallAddress();
                if (check != null)
                {
                    return this.Ok(new { Status = true, Message = "Address Retrived Successfully", Data = check});
                }
                else
                {
                    return this.Ok(new { Status = false, Message = "Address not Retrived" });
                }
            }
            catch (Exception e)
            {
                return this.Ok(new { Status = false, Message = e.Message });
            }
        }

        [HttpGet]
        [Route("getbyaddresstype")]
        public async Task<IActionResult> GetByAddressType(string addtypeId)
        {
            try
            {
                var check = await this.manager.GetByAddressType(addtypeId);
                if (check != null)
                {
                    return this.Ok(new ResponseModel<AddressModel> { Status = true, Message = "Address Retrived Successfully", Data = check });
                }
                else
                {
                    return this.Ok(new { Status = false, Message = "Address not Retrived" });
                }
            }
            catch (Exception e)
            {
                return this.Ok(new { Status = false, Message = e.Message });
            }
        }
    }
}
