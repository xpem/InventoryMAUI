using ApiRepos;
using Models.Item;
using Models.Resps;
using Services.Handlers;

namespace Services
{
    public interface IAcquisitionTypeService
    {
        Task<ServResp> GetAcquisitionType();
    }

    public partial class AcquisitionTypeService(IAcquisitionTypeApiRepo acquisitionTypeApiRepo) : IAcquisitionTypeService
    {
        public async Task<ServResp> GetAcquisitionType()
        {
            ApiResp resp = await acquisitionTypeApiRepo.GetAcquisitionType();

            return ApiRespHandler.Handler<List<AcquisitionType>>(resp);
        }
    }
}
