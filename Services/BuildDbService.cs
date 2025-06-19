using LocalRepos;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using Services.Interfaces;

namespace Services
{
    public class BuildDbService(IDbContextFactory<InventoryDbContext> DbCtx) : IBuildDbService
    {
        public void Init()
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();
            bool exists = Directory.Exists(FilePaths.DbPath);

            if (!exists)
                Directory.CreateDirectory(FilePaths.DbPath);

            context.Database.EnsureCreated();

            VersionDbTablesDTO? actualVesionDbTables = context.VersionDbTables.FirstOrDefault();

            VersionDbTablesDTO newVersionDbTables = new() { Id = 0, VERSION = 13 };

            if (actualVesionDbTables != null)
            {
                if (actualVesionDbTables.VERSION != newVersionDbTables.VERSION)
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    actualVesionDbTables.VERSION = newVersionDbTables.VERSION;

                    context.VersionDbTables.Add(actualVesionDbTables);

                    context.SaveChanges();
                }
            }
            else
            {
                context.VersionDbTables.Add(newVersionDbTables);
                context.SaveChanges();
            }
        }

        public async Task CleanLocalDatabase()
        {
            using InventoryDbContext context = DbCtx.CreateDbContext();
            context.User.RemoveRange(context.User);
            context.SubCategory.RemoveRange(context.SubCategory);

            await context.SaveChangesAsync();
        }
    }
}
