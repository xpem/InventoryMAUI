using ApiRepos.Interfaces;
using Models;
using Models.DTO;
using Models.Resps;
using System.Text.Json;

namespace ApiRepos
{
    public class SubCategoryApiRepo(IHttpClientFunctions httpClientFunctions) : ISubCategoryApiRepo
    {
        public async Task<ApiResp> GetSubCategoriesByCategoryId(string subCategoryId) =>
            await httpClientFunctions.AuthRequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/subcategory/category/" + subCategoryId);

        public async Task<ApiResp> GetSubCategoryById(string id) =>
            await httpClientFunctions.AuthRequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/subcategory/" + id);

        public async Task<ApiResp> UpdateApiAsync(SubCategoryDTO subCategory)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { subCategory.Name, subCategory.IconName, });

                return await httpClientFunctions.AuthRequestAsync(RequestsTypes.Put, ApiKeys.ApiAddress + "/Inventory/subcategory/" + subCategory.Id, json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> CreateAsync(SubCategoryDTO subCategory)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { subCategory.Name, subCategory.IconName, subCategory.CategoryId });

                return await httpClientFunctions.AuthRequestAsync(RequestsTypes.Post, ApiKeys.ApiAddress + "/Inventory/subcategory", json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> DelSubCategory(int id) =>
            await httpClientFunctions.AuthRequestAsync(RequestsTypes.Delete, ApiKeys.ApiAddress + "/Inventory/subCategory/" + id);

        public async Task<ApiResp> GetByLastUpdateAsync(DateTime lastUpdate, int page) =>
            await httpClientFunctions.AuthRequestAsync(RequestsTypes.Get, $"{ApiKeys.ApiAddress}/Inventory/subCategory/byAfterUpdatedAt/{lastUpdate:yyyy-MM-ddThh:mm:ss.fff}/{page}");


    }
}
