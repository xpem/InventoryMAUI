using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalRepos
{
    public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
    {
        public virtual required DbSet<VersionDbTablesDTO> VersionDbTables { get; set; }

        public virtual required DbSet<UserDTO> User { get; set; }

        public virtual required DbSet<SubCategoryDTO> SubCategory { get; set; }

        public virtual DbSet<ApiOperationDTO> ApiOperationQueue { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={Path.Combine(FilePaths.DbPath, "Inventory.db")}");
        }
    }
}
