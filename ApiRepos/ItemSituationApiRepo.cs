using Models.Resps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.Model;

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
