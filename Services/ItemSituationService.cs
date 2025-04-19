using ApiRepos;
using Models.Item;
using Models.Resps;
using Services.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.Model;

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
