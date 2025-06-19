using LocalRepos;
using Models;
using Models.Item.Files;
using Models.Resps;
using System.Net;

namespace ApiRepos
{
    public interface IHttpClientWithFileFunctions : IHttpClientFunctions;

    public class HttpClientWithFileFunctions(InventoryDbContext inventoryDbContextRepo) : HttpClientFunctions(inventoryDbContextRepo), IHttpClientFunctions, IHttpClientWithFileFunctions
    {
        public override async Task<ApiResp> RequestAsync(RequestsTypes requestsType, string url, string? userToken = null, Object? content = null)
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

                        string fileName = httpResponse.Content.Headers.ContentDisposition?.FileName ?? throw new ArgumentNullException("filename in headers not found!");

                        Stream resultStream = await httpResponse.Content.ReadAsStreamAsync();

                        return new ApiResp()
                        {
                            Success = httpResponse.IsSuccessStatusCode,
                            Error = httpResponse.StatusCode == HttpStatusCode.Unauthorized ? Models.Resps.ErrorTypes.Unauthorized : null,
                            TryRefreshToken = httpResponse.StatusCode == HttpStatusCode.Unauthorized,
                            Content = resultStream
                        };
                    case RequestsTypes.Put:
                        using (MultipartFormDataContent form = [])
                        {
                            if (content is not null and ItemFilesToUpload itemFilesToUpload)
                            {

                                if (itemFilesToUpload.Image1 != null)
                                {
                                    using FileStream fs = new FileStream(itemFilesToUpload.Image1.ImageFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                    using MemoryStream memoryStream = new();
                                    fs.CopyTo(memoryStream);

                                    ByteArrayContent fileContent = new ByteArrayContent(memoryStream.ToArray());
                                    //fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(itemFilesToUpload.Image1.FileContentType);

                                    form.Add(fileContent, "file1", itemFilesToUpload.Image1.FileName);
                                }

                                if (itemFilesToUpload.Image2 != null)
                                {
                                    using FileStream fs = new FileStream(itemFilesToUpload.Image2.ImageFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                    using MemoryStream memoryStream = new();
                                    fs.CopyTo(memoryStream);

                                    ByteArrayContent fileContent = new ByteArrayContent(memoryStream.ToArray());
                                    //fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(itemFilesToUpload.Image2.FileContentType);

                                    form.Add(fileContent, "file2", itemFilesToUpload.Image2.FileName);
                                }

                                HttpResponseMessage response = await httpClient.PutAsync(url, form);
                                response.EnsureSuccessStatusCode();
                                string responseContent = await response.Content.ReadAsStringAsync();

                                return new ApiResp() { Success = true, Content = responseContent };
                            }
                            else throw new InvalidCastException(nameof(content));
                        }
                }
            }
            catch (Exception ex)
            {
                return ex.InnerException is not null && (ex.InnerException.Message == "Nenhuma conexão pôde ser feita porque a máquina de destino as recusou ativamente." || ex.InnerException.Message.Contains("Este host não é conhecido."))
                    ? new ApiResp() { Success = false, Content = null, Error = Models.Resps.ErrorTypes.ServerUnavaliable }
                    : throw ex;
            }

            throw new Exception("Retorno não esperado");
        }

    }
}
