using Models.DTO;
using Models.Resps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISubCategoryService : ISyncHelperService
    {
        Task<ServResp> CreateApiAsync(SubCategoryDTO subCategory);
        Task<ServResp> DelSubCategory(int id);
        Task<ServResp> GetSubCategoriesByCategoryId(int categoryId);

        Task<List<SubCategoryDTO>> GetByCategoryIdAsync(int uid, int page, int categoryId);

        Task<ServResp> CreateAsync(int uid, bool isON, SubCategoryDTO subCategoryDTO);

        Task<ServResp> UpdateAsync(int uid, bool isOn, SubCategoryDTO subCategoryDTO);

    }
}
