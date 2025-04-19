using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public record CategoryDTO
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Color { get; set; }

        public bool? SystemDefault { get; set; }

        public List<SubCategoryDTO>? SubCategories { get; set; }

        /// <summary>
        /// used in get item
        /// </summary>
        public SubCategoryDTO? SubCategory { get; set; }
    }
}
