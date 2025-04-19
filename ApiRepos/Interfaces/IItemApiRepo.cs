using Models.Item;
using Models.Item.Files;
using Models.Resps;

namespace ApiRepos.Interfaces
{
    public interface IItemApiRepo
    {
        Task<ApiResp> AddItemAsync(Item item);
        Task<ApiResp> AddItemImage(int id, ItemFilesToUpload itemFilesToUpload);
        Task<ApiResp> AltItemAsync(Item item);
        Task<ApiResp> DelItemAsync(int id);
        Task<ApiResp> DelItemImageAsync(int id, string fileName);
        Task<ApiResp> GetItemByIdAsync(string id);
        Task<ApiResp> GetItemImageAsync(int id, string fileName);
        Task<ApiResp> GetPaginatedItemsAsync(int page);
        Task<ApiResp> GetTotalItensAsync();
    }
}