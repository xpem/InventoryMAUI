using Models.DTO;
using Models.Resps;

namespace ApiRepos.Interfaces
{
    public interface ISubCategoryApiRepo
    {
        Task<ApiResp> CreateAsync(SubCategoryDTO subCategory);
        Task<ApiResp> DelSubCategory(int id);
        Task<ApiResp> GetByLastUpdateAsync(DateTime lastUpdate, int page);
        Task<ApiResp> GetSubCategoriesByCategoryId(string subCategoryId);
        Task<ApiResp> GetSubCategoryById(string id);
        Task<ApiResp> UpdateApiAsync(SubCategoryDTO subCategory);
    }
}