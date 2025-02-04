using LocalRepos.Interface;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalRepos
{
    public class UserRepo(IDbContextFactory<InventoryDbContext> DbCtx) : IUserRepo
    {
        public async Task<UserDTO?> GetUserLocalAsync()
        {
            using var context = DbCtx.CreateDbContext();
            return await context.User.FirstOrDefaultAsync();
        }

        public void RemoveUserLocal()
        {
            using var context = DbCtx.CreateDbContext();
            _ = context.Set<UserDTO>().ExecuteDeleteAsync();
        }

        public async Task<int?> GetUidAsync()
        {
            using var context = DbCtx.CreateDbContext();

            return await context.User.Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<int> AddUserAsync(UserDTO user)
        {
            using var context = DbCtx.CreateDbContext();

            context.User.Add(user);

            return await context.SaveChangesAsync();
        }

        public int ExecuteUpdateLastUpdateUser(DateTime lastUpdate, int uid)
        {
            using var context = DbCtx.CreateDbContext();

            return context.User.Where(x => x.Id == uid).ExecuteUpdate(y => y.SetProperty(z => z.LastUpdate, lastUpdate));
        }

    }
}
