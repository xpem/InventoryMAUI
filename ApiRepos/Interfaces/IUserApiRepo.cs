using Models.Resps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRepos.Interfaces
{
    public interface IUserApiRepo
    {
        Task<ApiResp> AddUserAsync(string name, string email, string password);
        Task<ApiResp> GetUserAsync(string token);
        Task<(bool, string?)> GetUserTokenAsync(string email, string password);
        Task<ApiResp> RecoverPasswordAsync(string email);
    }
}
