using Models.DTO;
using Models.Resps;

namespace Services.Interfaces
{
    public interface IUserService
    {
        ServResp AddUser(string name, string email, string password);
        Task<ServResp> SignIn(string email, string password);

        Task<UserDTO?> GetAsync();

        Task<(bool, string?)> GetUserTokenAsync(string email, string password);

        Task<string?> RecoverPasswordAsync(string email);

        void UpdateLastUpdate(int uid);

        void RemoveUserLocal();
    }
}
