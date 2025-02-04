using ApiRepos.Interfaces;
using Models.Resps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRepos
{
    public class UserApiRepo : IUserApiRepo
    {
        private readonly UsersManagement.UserService userService = new(ApiKeys.ApiAddress);

        public async Task<ApiResp> AddUserAsync(string name, string email, string password)
        {
            try
            {
                UsersManagement.Model.ApiResponse resp = await userService.AddUserAsync(name, email, password);

                return new() { Success = resp.Success, Content = resp.Content, Error = (ErrorTypes?)resp.Error };
            }
            catch { throw; }
        }

        public async Task<ApiResp> RecoverPasswordAsync(string email)
        {
            UsersManagement.Model.ApiResponse resp = await userService.RecoverPasswordAsync(email);

            return new() { Success = resp.Success, Content = resp.Content, Error = (ErrorTypes?)resp.Error };
        }

        public async Task<(bool, string?)> GetUserTokenAsync(string email, string password)
        {
            UsersManagement.Model.ApiResponse resp = await userService.GetUserTokenAsync(email, password);

            return (resp.Success, resp.Content);
        }

        public async Task<ApiResp> GetUserAsync(string token)
        {
            try
            {
                UsersManagement.Model.ApiResponse resp = await userService.GetUserAsync(token);

                return new() { Success = resp.Success, Content = resp.Content, Error = (ErrorTypes?)resp.Error };
            }
            catch { throw; }
        }
    }
}
