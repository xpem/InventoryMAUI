using ApiRepos.Handlers;
using ApiRepos.Interfaces;
using LocalRepos.Interface;
using Models.DTO;
using Models.Resps;
using Services.Interfaces;
using System.Text.Json.Nodes;

namespace Services
{
    public class UserService(IUserRepo userRepo, IUserApiRepo userApiDAL) : IUserService
    {
        public ServResp AddUser(string name, string email, string password)
        {
            email = email.ToLower();
            ApiResp? resp = userApiDAL.AddUserAsync(name, email, password).Result;

            if (resp is not null && resp.Success && resp.Content is not null and string)
            {
                JsonNode? jResp = JsonNode.Parse(resp.Content as string);

                if (jResp is not null)
                {
                    UserDTO user = new()
                    {
                        Id = jResp["id"]?.GetValue<int>() ?? 0,
                        Name = jResp["name"]?.GetValue<string>(),
                        Email = jResp["email"]?.GetValue<string>()
                    };

                    if (user.Id is not 0)
                        return new ServResp() { Success = resp.Success };
                }
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<string?> RecoverPasswordAsync(string email)
        {
            email = email.ToLower();
            ApiResp? resp = await userApiDAL.RecoverPasswordAsync(email);

            if (resp is not null && resp.Content is not null and string)
            {
                JsonNode? jResp = JsonNode.Parse(resp.Content as string);
                if (jResp is not null)
                    return jResp["Mensagem"]?.GetValue<string>();
            }

            return null;
        }

        public async Task<(bool, string?)> GetUserTokenAsync(string email, string password) => await userApiDAL.GetUserTokenAsync(email.ToLower(), password);

        public async Task<UserDTO?> GetAsync() => await userRepo.GetUserLocalAsync();

        public void RemoveUserLocal() => userRepo.RemoveUserLocal();

        public async Task<ServResp> SignIn(string email, string password)
        {
            try
            {
                email = email.ToLower();

                (bool success, string? userTokenRes) = await GetUserTokenAsync(email, password);

                if (success && userTokenRes != null)
                {
                    ApiResp resp = await userApiDAL.GetUserAsync(userTokenRes);

                    if (resp.Success && resp.Content is not null and string)
                    {
                        JsonNode? userResponse = JsonNode.Parse((string)resp.Content);

                        if (userResponse is not null)
                        {
                            UserDTO? user = new()
                            {
                                Id = userResponse["id"]?.GetValue<int>() ?? 0,
                                Name = userResponse["name"]?.GetValue<string>(),
                                Email = userResponse["email"]?.GetValue<string>(),
                                Token = userTokenRes,
                                Password = EncryptionService.Encrypt(password)
                            };

                            _ = userRepo.AddUserAsync(user);

                            return new ServResp() { Success = true, Content = user.Id };
                        }
                    }
                }
                //maybe use a errorcodes instead a message?
                else return !success && userTokenRes is not null && userTokenRes == "User/Password incorrect"
                    ? new ServResp() { Success = false, Error = ErrorTypes.WrongEmailOrPassword }
                    : throw new Exception("Erro não mapeado");

                return new ServResp() { Success = false, Error = ErrorTypes.Unknown };
            }
            catch { throw; }
        }

        public void UpdateLastUpdate(int uid) => userRepo.ExecuteUpdateLastUpdateUser(DateTime.Now, uid);

    }
}
