using ManagerLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;

        private readonly ILogger<UserController> logger;

        public UserController(IUserManager manager, ILogger<UserController> logger)
        {
            this.manager = manager;
            this.logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            try
            {
                this.logger.LogInformation(register.fullName + " is trying to Register");
                var resp = await this.manager.Register(register);
                if(resp != null)
                {
                    this.logger.LogInformation(register.fullName + " has Registered Successfully");
                    return this.Ok(new ResponseModel<RegisterModel> { Status = true, Message = "User Registered Successful", Data=resp});
                }
                else
                {
                    this.logger.LogInformation(register.fullName + " is not Registered");
                    return this.BadRequest(new { Status = false, Message = "User not Registered" });
                }
            }
            catch(Exception e)
            {
                this.logger.LogInformation(register.fullName + " has an Exception in Register");
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            try
            {
                this.logger.LogInformation(login.emailID + " is trying to Login");
                var response = await this.manager.Login(login);
                if (response != null)
                {
                    ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                    IDatabase database = connectionMultiplexer.GetDatabase();
                    string fullName = database.StringGet("FullName");
                    string email = database.StringGet("Email");
                    double mobile = (double)database.StringGet("Mobile");
                    string userId = database.StringGet("UserId");

                    RegisterModel data = new RegisterModel
                    {
                        fullName = fullName,
                        userID = userId,
                        emailID = login.emailID,
                        mobile = mobile
                    };
                    string token = this.manager.GenerateToken(login.emailID);
                    this.logger.LogInformation(login.emailID + " has Login Successfully");
                    return this.Ok(new { Status = true, Message = "Login Successfully", Data = data, Token = token });
                }
                else
                {
                    this.logger.LogInformation(login.emailID + " is not Logged in");
                    return this.BadRequest(new { Status = false, Message = "Login Unsuccessful" });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation(login.emailID + " has an Exception in Login");
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("forget")]
        public async Task<IActionResult> Forget(string email)
        {
            try
            {
                this.logger.LogInformation(email + " is trying to send reset link");
                var resp = await this.manager.Forget(email);
                if (resp == true)
                {
                    this.logger.LogInformation(email + " has sent Link successfully");
                    return this.Ok(new { Status = true, Message = "Link Send Successfully", Data = resp });
                }
                else
                {
                    this.logger.LogInformation(email + " cannot send Link");
                    return this.BadRequest(new { Status = false, Message = "Link not Sent" });
                }
            }
            catch (Exception e)
            {
                this.logger.LogInformation(email + " has an Exception in sending Link");
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> Reset([FromBody] ResetModel reset)
        {
            try
            {
                this.logger.LogInformation(reset.emailID + "is trying to Reset Password");
                var resp = await this.manager.Reset(reset);
                if (resp != null)
                {
                    this.logger.LogInformation(reset.emailID + " has Reset Password Successfully");
                    return this.Ok(new ResponseModel<RegisterModel> { Status = true, Message = "User Password Reset Successful", Data = resp });
                }
                else
                {
                    this.logger.LogInformation(reset.emailID + " cannot not be Reset Password");
                    return this.BadRequest(new { Status = false, Message = "User Password not Reset" });
                }
            }
            catch (Exception e)
            {
                this.logger.LogInformation(reset.emailID + " has an Exception in Resetting Password");
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }
    }
}
