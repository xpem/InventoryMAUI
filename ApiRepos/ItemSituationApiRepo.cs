using Models.Resps;

namespace ApiRepos
{
    public interface IItemSituationApiRepo
    {
        Task<ApiResp> GetItemSituation();
    }

    public class ItemSituationApiRepo(IHttpClientFunctions httpClientFunctions) : IItemSituationApiRepo
    {
        public async Task<ApiResp> GetItemSituation()
            => await httpClientFunctions.AuthRequestAsync(Models.RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/itemsituation");
    }
}
