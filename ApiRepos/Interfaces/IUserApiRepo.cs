using Models.Resps;

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
