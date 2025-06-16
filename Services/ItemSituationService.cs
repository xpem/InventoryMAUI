using ApiRepos;
using Models.Item;
using Models.Resps;
using Services.Handlers;

namespace Services
{
    public interface IItemSituationService
    {
        Task<ServResp> GetItemSituation();
    }

    public class ItemSituationService(IItemSituationApiRepo itemSituationApiRepo) : IItemSituationService
    {
        public async Task<ServResp> GetItemSituation()
        {
            ApiResp resp = await itemSituationApiRepo.GetItemSituation();

            return ApiRespHandler.Handler<List<ItemSituation>>(resp);
        }
    }
}
