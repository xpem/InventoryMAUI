using LocalRepos.Interface;
using Microsoft.EntityFrameworkCore;
using Models.DTO;

namespace LocalRepos
{
    public class SubCategoryRepo(IDbContextFactory<InventoryDbContext> DbCtx) : ISubCategoryRepo
    {
        readonly int pageSize = 10;

        public async Task<DateTime?> GetUpdatedAtByIdAsync(int id)
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();
            return (await context.SubCategory.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync())?.UpdatedAt;
        }

        public async Task<List<SubCategoryDTO>> GetByCategoryIdAsync(int uid, int page, int categoryId)
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();
            return await context.SubCategory.Where(x => x.UserId == uid && x.CategoryId.Equals(categoryId))
                .OrderByDescending(x => x.UpdatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<bool> CheckIfExistsByCategoryIdAndName(int uid, int categoryId, string name, int? localId)
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();

            return localId.HasValue
                ? await context.SubCategory.AnyAsync(x => x.UserId == uid && x.CategoryId.Equals(categoryId) && x.Name == name && x.LocalId != localId)
                : await context.SubCategory.AnyAsync(x => x.UserId == uid && x.CategoryId.Equals(categoryId) && x.Name == name);
        }

        public async Task<int> UpdateAsync(SubCategoryDTO subCategory)
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();

            context.SubCategory.Update(subCategory);

            return await context.SaveChangesAsync();
        }

        public async Task<int> CreateAsync(SubCategoryDTO subCategory)
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();
            await context.SubCategory.AddAsync(subCategory);

            return await context.SaveChangesAsync();
        }

        public async Task<SubCategoryDTO?> GetByIdAsync(int id)
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();
            return await context.SubCategory.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<SubCategoryDTO?> GetByLocalIdAsync(int uid, int localId)
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();
            return await context.SubCategory.Where(x => x.UserId == uid && x.LocalId == localId).FirstOrDefaultAsync();
        }

    }
}
