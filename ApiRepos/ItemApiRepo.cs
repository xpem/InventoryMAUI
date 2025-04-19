using ApiRepos.Interfaces;
using Models;
using Models.Item;
using Models.Item.Files;
using Models.Resps;
using System.Text.Json;

namespace ApiRepos
{
    public class ItemApiRepo(IHttpClientFunctions httpClientFunctions, IHttpClientWithFileFunctions httpClientWithFileFunctions) : IItemApiRepo
    {
        public async Task<ApiResp> GetTotalItensAsync() => await httpClientFunctions.AuthRequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/item/totals");

        public async Task<ApiResp> GetPaginatedItemsAsync(int page) =>
            await httpClientFunctions.AuthRequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/item?page=" + page);

        public async Task<ApiResp> GetItemByIdAsync(string id) =>
           await httpClientFunctions.AuthRequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/item/" + id);

        public async Task<ApiResp> GetItemImageAsync(int id, string fileName) =>
            await httpClientWithFileFunctions.AuthRequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/item/" + id + "/image/" + fileName);

        public async Task<ApiResp> AddItemImage(int id, ItemFilesToUpload itemFilesToUpload) =>
            await httpClientWithFileFunctions.AuthRequestAsync(Models.RequestsTypes.Put, ApiKeys.ApiAddress + "/Inventory/item/" + id + "/image", itemFilesToUpload);

        public async Task<ApiResp> DelItemImageAsync(int id, string fileName) =>
           await httpClientFunctions.AuthRequestAsync(RequestsTypes.Delete, $"{ApiKeys.ApiAddress}/Inventory/item/{id}/image/{fileName}");

        public async Task<ApiResp> AddItemAsync(Item item)
        {
            string json = BuildItemJson(item);

            return await httpClientFunctions.AuthRequestAsync(Models.RequestsTypes.Post, ApiKeys.ApiAddress + "/Inventory/item", json);
        }

        private static string BuildItemJson(Item item) =>
            JsonSerializer.Serialize(new
            {
                item.Name,
                item.TechnicalDescription,
                AcquisitionDate = DateOnly.FromDateTime(item.AcquisitionDate),
                item.PurchaseValue,
                item.PurchaseStore,
                item.ResaleValue,
                SituationId = item.Situation?.Id,
                item.Comment,
                AcquisitionType = item.AcquisitionType?.Id,
                Category = new { CategoryId = item.Category?.Id, SubCategoryId = item.Category?.SubCategory is not null ? (int?)item.Category.SubCategory.Id : null },
                WithdrawalDate = item.WithdrawalDate != null ? (DateOnly?)DateOnly.FromDateTime(item.WithdrawalDate.Value) : null,
            });

        public async Task<ApiResp> AltItemAsync(Item item)
        {
            try
            {
                string json = BuildItemJson(item);

                return await httpClientFunctions.AuthRequestAsync(Models.RequestsTypes.Put, ApiKeys.ApiAddress + "/Inventory/item/" + item.Id, json);
            }
            catch (Exception ex) { throw; }
        }

        public async Task<ApiResp> DelItemAsync(int id)
        {
            try
            {
                return await httpClientFunctions.AuthRequestAsync(RequestsTypes.Delete, ApiKeys.ApiAddress + "/Inventory/item/" + id);
            }
            catch (Exception ex) { throw; }
        }
    }
}
