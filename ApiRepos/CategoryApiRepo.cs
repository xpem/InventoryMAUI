using Models;
using Models.DTO;
using Models.Resps;
using System.Text.Json;

namespace ApiRepos
{
    public interface ICategoryApiRepo
    {
        Task<ApiResp> AddCategoryAsync(CategoryDTO category);
        Task<ApiResp> AltCategoryAsync(CategoryDTO category);
        Task<ApiResp> DelCategoryAsync(int id);
        Task<ApiResp> GetCategoriesAsync();
        Task<ApiResp> GetCategoriesWithSubCategoriesAsync();
        Task<ApiResp> GetCategoryByIdAsync(string id);
    }

    public class CategoryApiRepo(IHttpClientFunctions httpClientFunctions) : ICategoryApiRepo
    {
        public async Task<ApiResp> GetCategoriesAsync() =>
          await httpClientFunctions.AuthRequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/category");

        public async Task<ApiResp> GetCategoriesWithSubCategoriesAsync() =>
            await httpClientFunctions.AuthRequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/category/subcategory");

        public async Task<ApiResp> GetCategoryByIdAsync(string id) =>
            await httpClientFunctions.AuthRequestAsync(RequestsTypes.Get, ApiKeys.ApiAddress + "/Inventory/category/" + id);

        public async Task<ApiResp> AddCategoryAsync(CategoryDTO category)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { category.Name, category.Color });

                return await httpClientFunctions.AuthRequestAsync(RequestsTypes.Post, ApiKeys.ApiAddress + "/Inventory/category", json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> AltCategoryAsync(CategoryDTO category)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { category.Name, category.Color });

                return await httpClientFunctions.AuthRequestAsync(RequestsTypes.Put, ApiKeys.ApiAddress + "/Inventory/category/" + category.Id, json);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ApiResp> DelCategoryAsync(int id)
        {
            try
            {
                return await httpClientFunctions.AuthRequestAsync(RequestsTypes.Delete, ApiKeys.ApiAddress + "/Inventory/category/" + id);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
