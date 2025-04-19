using Models.Item;
using Models.Item.Files;
using Models.Resps;

namespace Services.Interfaces
{
    public interface IItemService
    {
        Task<ServResp> AddItemAsync(Item item);
        Task<ServResp> AddItemImageAsync(int id, ItemFilesToUpload itemFilesToUpload);
        Task<ServResp> AltItemAsync(Item item);
        Task<ServResp> DelItemAsync(int id);
        Task<ServResp> DelItemImageAsync(int id, string filename);
        Task<ServResp> GetItemByIdAsync(string id);
        Task<ItemFilesToUpload> GetItemImages(int itemId, string itemImage1, string itemImage2);
        Task<List<Item>> GetItemsAllAsync();
    }
}