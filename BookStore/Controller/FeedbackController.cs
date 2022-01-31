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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackManager manager;
        public FeedbackController(IFeedbackManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("addfeedback")]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackModel feed)
        {
            try
            {
                var check = await this.manager.AddFeedback(feed);
                if (check != null)
                {
                    return this.Ok(new ResponseModel<FeedbackModel> { Status = true, Message = "Feedback Added", Data = check });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Feedback not Added" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }


        [HttpGet]
        [Route("getfeedback")]
        public IActionResult GetFeedback()
        {
            try
            {
                IEnumerable<FeedbackModel> check = this.manager.GetFeedback();
                if (check != null)
                {
                    return this.Ok(new { Status = true, Message = "Feedback Retrived Successfully", Data = check });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Feedback is Empty" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }
    }
}
