using Models.Resps;

namespace ApiRepos
{
    public interface IAcquisitionTypeApiRepo
    {
        Task<ApiResp> GetAcquisitionType();
    }

    public class AcquisitionTypeApiRepo(IHttpClientFunctions httpClientFunctions) : IAcquisitionTypeApiRepo
    {
        public async Task<ApiResp> GetAcquisitionType() =>
            await httpClientFunctions.AuthRequestAsync(Models.RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/acquisitiontype");
    }
}
