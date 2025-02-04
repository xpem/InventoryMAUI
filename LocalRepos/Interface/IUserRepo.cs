using Models.DTO;

namespace LocalRepos.Interface
{
    public interface IUserRepo
    {
        Task<int> AddUserAsync(UserDTO user);

        int ExecuteUpdateLastUpdateUser(DateTime lastUpdate, int uid);

        Task<UserDTO?> GetUserLocalAsync();

        Task<int?> GetUidAsync();

        void RemoveUserLocal();
    }
}
