using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO
{
    [Table("SubCategory")]
    public class SubCategoryDTO : DTOBase
    {
        public string? Name { get; set; }

        public string? IconName { get; set; }

        public bool? SystemDefault { get; set; }

        public int CategoryId { get; set; }
    }
}
