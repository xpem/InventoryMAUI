using Models.Resps;
using System.Text.Json;

namespace Services.Handlers
{
    public static class ApiRespHandler
    {
        public static ServResp Handler<TModel>(ApiResp apiResponse)
        {
            try
            {
                if (apiResponse is not null)
                {
                    if (!apiResponse.Success)
                    {
                        if (apiResponse.Error != null)
                            return new ServResp() { Success = false, TryRefreshToken = apiResponse.TryRefreshToken, Error = apiResponse.Error };
                        else if (apiResponse.Content != null)
                            return new ServResp() { Success = false, TryRefreshToken = apiResponse.TryRefreshToken, Content = apiResponse.Content };
                    }

                    return apiResponse.Content is not null and string
                        ? new ServResp()
                        {
                            Success = true,
                            Content = string.IsNullOrEmpty(apiResponse.Content as string) ? null : JsonDeserialize<TModel>(apiResponse.Content as string)
                        }
                        : throw new Exception("apiResponse.Content nulo");
                }

                throw new Exception("apiResponse nulo");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static TModel JsonDeserialize<TModel>(string content)
        {
            try
            {
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                TModel? item = JsonSerializer.Deserialize<TModel>(content, options);
                return item is not null ? item : throw new Exception("item nulo");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
