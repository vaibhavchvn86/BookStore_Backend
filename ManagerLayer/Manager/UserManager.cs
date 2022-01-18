using ManagerLayer.Interface;
using ModelLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository repo;
        public UserManager(IUserRepository repo)
        {
            this.repo = repo;
        }

        public async Task<RegisterModel> Register(RegisterModel register)
        {
            register.password = EncodePasswordToBase64(register.password);
            try
            {
                return await this.repo.Register(register);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RegisterModel> Login(LoginModel login)
        {
            login.password = EncodePasswordToBase64(login.password);
            try
            {
                return await this.repo.Login(login);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> Forget(string email)
        {
            try 
            {
                return await this.repo.Forget(email);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RegisterModel> Reset(ResetModel reset)
        {
            reset.newpassword = EncodePasswordToBase64(reset.newpassword);
            try
            {
                return await this.repo.Reset(reset);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public string GenerateToken(string email)
        {
            try
            {
                return this.repo.GenerateToken(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
