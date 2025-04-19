using ApiRepos.Handlers;
using LocalRepos;
using Models;
using Models.DTO;
using Models.Exceptions;
using Models.Resps;
using System.Net;
using System.Text;

namespace ApiRepos
{
    public interface IHttpClientFunctions
    {
        Task<ApiResp> AuthRequestAsync(RequestsTypes requestsType, string url, Object? content = null);
        Task<bool> CheckServerAsync();
        Task<ApiResp> RequestAsync(RequestsTypes requestsType, string url, string? userToken = null, Object? content = null);
    }

    public class HttpClientFunctions(InventoryDbContext inventoryDbContextRepo) : HttpClient, IHttpClientFunctions
    {
        public async Task<bool> CheckServerAsync()
        {
            try
            {
                HttpClient httpClient = new();

                HttpResponseMessage httpResponse = await httpClient.GetAsync(ApiKeys.ApiAddress + "/imalive");

                if (httpResponse != null && httpResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(await httpResponse.Content.ReadAsStringAsync())) return true;

                return false;
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null && (ex.InnerException.Message == "Nenhuma conexão pôde ser feita porque a máquina de destino as recusou ativamente." || ex.InnerException.Message.Contains("Este host não é conhecido.")))
                    return false;

                throw ex;
            }
        }

        public virtual async Task<ApiResp> RequestAsync(RequestsTypes requestsType, string url, string? userToken = null, Object? content = null)
        {
            try
            {
                HttpClient httpClient = new();

                if (userToken is not null)
                    httpClient.DefaultRequestHeaders.Add("authorization", "bearer " + userToken);

                HttpResponseMessage httpResponse = new();

                switch (requestsType)
                {
                    case RequestsTypes.Get:
                        httpResponse = await httpClient.GetAsync(url);
                        break;
                    case RequestsTypes.Post:
                        if (content is not null)
                        {
                            string jsonContent = content as string;

                            StringContent bodyContent = new(jsonContent, Encoding.UTF8, "application/json");
                            httpResponse = await httpClient.PostAsync(url, bodyContent);
                        }
                        else return new ApiResp() { Success = false, Content = null, Error = ErrorTypes.BodyContentNull };
                        break;
                    case RequestsTypes.Put:
                        if (content is not null)
                        {
                            string jsonContent = content as string;
                            StringContent bodyContent = new(jsonContent, Encoding.UTF8, "application/json");
                            httpResponse = await httpClient.PutAsync(url, bodyContent);
                        }
                        else return new ApiResp() { Success = false, Content = null, Error = ErrorTypes.BodyContentNull };
                        break;
                    case RequestsTypes.Delete:
                        httpResponse = await httpClient.DeleteAsync(url);
                        break;
                }

                return new ApiResp()
                {
                    Success = httpResponse.IsSuccessStatusCode,
                    Error = httpResponse.StatusCode == HttpStatusCode.Unauthorized ? ErrorTypes.Unauthorized : null,
                    TryRefreshToken = httpResponse.StatusCode == HttpStatusCode.Unauthorized,
                    Content = await httpResponse.Content.ReadAsStringAsync()
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null && (ex.InnerException.Message == "Nenhuma conexão pôde ser feita porque a máquina de destino as recusou ativamente." || ex.InnerException.Message.Contains("Este host não é conhecido.")))
                    return new ApiResp() { Success = false, Content = null, Error = ErrorTypes.ServerUnavaliable };

                throw ex;
            }
        }

        public async Task<ApiResp> AuthRequestAsync(RequestsTypes requestsType, string url, Object? content = null)
        {
            bool retry = true;
            ApiResp? resp = null;

            while (retry)
            {
                string? userToken;

                if (resp is not null && resp.TryRefreshToken)
                {
                    retry = false;

                    (bool refreshTokenSuccess, userToken) = await RefreshToken();

                    if (!refreshTokenSuccess || userToken is null)
                        return resp;
                }
                else
                {
                    userToken = inventoryDbContextRepo.User.FirstOrDefault()?.Token;

                    if (userToken is null) throw new ArgumentNullException(nameof(userToken));
                }

                resp = await RequestAsync(requestsType, url, userToken, content);

                if (!resp.TryRefreshToken || !retry) return resp;
            }

            throw new Exception($"Erro ao tentar AuthRequest de tipo {requestsType} na url: {url}");
        }

        private async Task<(bool success, string? newToken)> RefreshToken()
        {
            UserDTO? user = inventoryDbContextRepo.User.FirstOrDefault();

            if (user is not null && user.Email is not null && user.Password is not null)
            {
                string password = EncryptionService.Decrypt(user.Password);

                UsersManagement.Model.ApiResponse resp = await new UsersManagement.UserService(ApiKeys.ApiAddress).GetUserTokenAsync(user.Email, password);

                if (resp.Content is not null)
                {
                    if (resp.Success)
                    {
                        string newToken = resp.Content;

                        user.Token = newToken;

                        inventoryDbContextRepo.Update(user);
                        await inventoryDbContextRepo.SaveChangesAsync();

                        return (true, newToken);
                    }
                    else
                        throw new SignInFailException(resp.Content);
                }
            }

            return (false, null);
        }
    }
}
