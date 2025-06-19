using Models.DTO;
using Models.Resps;

namespace Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ServResp> AddCategoryAsync(CategoryDTO category);
        Task<ServResp> AltCategoryAsync(CategoryDTO category);
        Task<ServResp> DelCategoryAsync(int id);
        Task<ServResp> GetCategoriesAsync();
        Task<ServResp> GetCategoriesWithSubCategoriesAsync();
        Task<ServResp> GetCategoryByIdAsync(string id);
    }
}