using ModelLayer;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Experimental.System.Messaging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StackExchange.Redis;

namespace RepositoryLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<RegisterModel> User;

        private readonly IConfiguration configuration;

        public UserRepository(IDBSetting db, IConfiguration configuration)
            {
            this.configuration = configuration;
            var userclient = new MongoClient(db.ConnectionString);
            var database = userclient.GetDatabase(db.DatabaseName);
            User = database.GetCollection<RegisterModel>("User");
            }
        public async Task<RegisterModel> Register(RegisterModel register)
        {
            try
            {
                var check = await this.User.AsQueryable().Where(x => x.emailID == register.emailID).FirstOrDefaultAsync();
                if (check != null)
                {
                    await this.User.InsertOneAsync(register);
                    return check;

                }
                return null;

            }
            catch(ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RegisterModel> Login(LoginModel login)
        {
            try
            {
                var check = await this.User.Find(x => x.emailID == login.emailID).FirstOrDefaultAsync();
                if (check != null)
                {
                    check = await this.User.Find(x => x.password == login.password).FirstOrDefaultAsync();
                    if (check != null)
                    {
                        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        IDatabase database = connectionMultiplexer.GetDatabase();
                        database.StringSet(key: "FullName", check.fullName);
                        database.StringSet(key: "Email", check.emailID);
                        database.StringSet(key: "Mobile", check.mobile);
                        database.StringSet(key: "UserId", check.userID);

                        return check;
                    }
                    return null;

                }
                return null;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> Forget(string email)
        {
            try
            {
                var check = await this.User.AsQueryable().Where(x => x.emailID == email).FirstOrDefaultAsync();
                if (check != null)
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress(this.configuration["Credentials:Email"]);
                    mail.To.Add(email);
                    mail.Subject = "Reset Password for BookStore";
                    this.SendMSMQ();
                    mail.Body = this.ReceiveMSMQ();

                    SmtpServer.Host = "smtp.gmail.com";
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(this.configuration["Credentials:Email"], this.configuration["Credentials:Password"]);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                    return true;

                }
                return false;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public void SendMSMQ()
        {
            MessageQueue msgqueue;
            if (MessageQueue.Exists(@".\Private$\BookStore"))
            {
                msgqueue = new MessageQueue(@".\Private$\BookStore");
            }
            else
            {
                msgqueue = MessageQueue.Create(@".\Private$\BookStore");
            }

            msgqueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            string body = "This is Password reset link. Reset Link => ";
            msgqueue.Label = "Mail Body";
            msgqueue.Send(body);
        }

        public string ReceiveMSMQ()
        {
            MessageQueue msgqueue = new MessageQueue(@".\Private$\BookStore");
            var receivemessage = msgqueue.Receive();
            receivemessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            return receivemessage.Body.ToString();
        }

        public async Task<RegisterModel> Reset(ResetModel reset)
        {
            try
            {
                var check = await this.User.AsQueryable().Where(x => x.emailID == reset.emailID).FirstOrDefaultAsync();
                if (check != null)
                {
                    await this.User.UpdateOneAsync(x => x.emailID == reset.emailID,
                        Builders<RegisterModel>.Update.Set(x => x.password, reset.password)); 
                    return check;

                }
                return null;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public string GenerateToken(string email)
        {
            byte[] key = Encoding.UTF8.GetBytes(this.configuration["SecretKey"]);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                      { new Claim(ClaimTypes.Email, email) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
