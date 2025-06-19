using LocalRepos.Interface;
using Microsoft.EntityFrameworkCore;
using Models.DTO;

namespace LocalRepos
{
    public class UserRepo(IDbContextFactory<InventoryDbContext> DbCtx) : IUserRepo
    {
        public async Task<UserDTO?> GetUserLocalAsync()
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();
            return await context.User.FirstOrDefaultAsync();
        }

        public void RemoveUserLocal()
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();
            _ = context.Set<UserDTO>().ExecuteDeleteAsync();
        }

        public async Task<int?> GetUidAsync()
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();

            return await context.User.Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<int> AddUserAsync(UserDTO user)
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();

            context.User.Add(user);

            return await context.SaveChangesAsync();
        }

        public int ExecuteUpdateLastUpdateUser(DateTime lastUpdate, int uid)
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();

            return context.User.Where(x => x.Id == uid).ExecuteUpdate(y => y.SetProperty(z => z.LastUpdate, lastUpdate));
        }

    }
}
