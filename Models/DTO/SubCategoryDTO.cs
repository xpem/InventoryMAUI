using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class SubCategoryDTO : DTOBase
    {
        public string? Name { get; set; }

        public string? IconName { get; set; }

        public bool? SystemDefault { get; set; }

        public int CategoryId { get; set; }

    }
}
