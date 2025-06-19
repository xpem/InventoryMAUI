using ApiRepos;
using Models.DTO;
using Models.Resps;
using Services.Handlers;
using Services.Interfaces;
using System.Text.Json.Nodes;

namespace Services
{
    public class CategoryService(ICategoryApiRepo categoryApiRepo) : ICategoryService
    {
        public async Task<ServResp> GetCategoriesAsync() => ApiRespHandler.Handler<List<CategoryDTO>>(await categoryApiRepo.GetCategoriesAsync());

        public async Task<ServResp> GetCategoriesWithSubCategoriesAsync() => ApiRespHandler.Handler<List<CategoryDTO>>(await categoryApiRepo.GetCategoriesWithSubCategoriesAsync());

        public async Task<ServResp> GetCategoryByIdAsync(string id) => ApiRespHandler.Handler<CategoryDTO>(await categoryApiRepo.GetCategoryByIdAsync(id));

        public async Task<ServResp> AddCategoryAsync(CategoryDTO category)
        {
            ApiResp? resp = await categoryApiRepo.AddCategoryAsync(category);

            if (resp is not null && resp.Content is not null and string)
            {
                JsonNode? jResp = JsonNode.Parse(resp.Content as string);
                if (resp.Success && jResp is not null)
                {
                    CategoryDTO categoryResp = new()
                    {
                        Id = jResp["Id"]?.GetValue<int>() ?? 0,
                        Name = jResp["Name"]?.GetValue<string>(),
                        Color = jResp["Color"]?.GetValue<string>(),
                        SystemDefault = jResp["SystemDefault"]?.GetValue<bool>()
                    };

                    return new ServResp() { Success = resp.Success, Content = categoryResp };
                }
                else return new ServResp() { Success = false, Content = resp.Content };
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> AltCategoryAsync(CategoryDTO category)
        {
            ApiResp? resp = await categoryApiRepo.AltCategoryAsync(category);

            if (resp is not null && resp.Content is not null and string)
            {
                if (resp.Success)
                {
                    JsonNode? jResp = JsonNode.Parse(resp.Content as string);
                    if (jResp is not null)
                    {
                        CategoryDTO categoryResp = new()
                        {
                            Id = jResp["Id"]?.GetValue<int>() ?? 0,
                            Name = jResp["Name"]?.GetValue<string>(),
                            Color = jResp["Color"]?.GetValue<string>(),
                            SystemDefault = jResp["SystemDefault"]?.GetValue<bool>()
                        };

                        return new ServResp() { Success = resp.Success, Content = categoryResp };
                    }
                }
                else return new ServResp() { Success = false, Content = resp.Content };
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> DelCategoryAsync(int id)
        {
            ApiResp? resp = await categoryApiRepo.DelCategoryAsync(id);

            return resp is not null && resp.Content is not null
                ? new ServResp() { Success = resp.Success, Content = resp.Content }
                : new ServResp() { Success = false, Content = null };
        }
    }
}
