using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalRepos.Interface
{
    public interface ISubCategoryRepo
    {
        Task<int> CreateAsync(SubCategoryDTO subCategory);
        Task<DateTime?> GetUpdatedAtByIdAsync(int id);
        Task<int> UpdateAsync(SubCategoryDTO subCategory);
        Task<SubCategoryDTO?> GetByIdAsync(int id);
        Task<SubCategoryDTO?> GetByLocalIdAsync(int uid, int localId);

        Task<List<SubCategoryDTO>> GetByCategoryIdAsync(int uid, int page, int categoryId);

        Task<bool> CheckIfExistsByCategoryIdAndName(int uid, int categoryId, string name, int? localId);

    }
}
