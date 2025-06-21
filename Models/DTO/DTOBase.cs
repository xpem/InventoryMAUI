using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class DTOBase
    {
        public int? Id { get; set; }

        [Key]
        public int? LocalId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Inactive { get; set; }
    }
}
