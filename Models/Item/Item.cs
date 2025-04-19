using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Item
{
    public record Item
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? TechnicalDescription { get; set; }

        public DateTime AcquisitionDate { get; set; }

        public AcquisitionType? AcquisitionType { get; set; }

        public decimal? PurchaseValue { get; set; }

        public string? PurchaseStore { get; set; }

        public decimal? ResaleValue { get; set; }

        public ItemSituation? Situation { get; set; }

        public string? Comment { get; set; }

        public CategoryDTO? Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? WithdrawalDate { get; set; }

        public string? Image1 { get; set; }

        public string? Image2 { get; set; }
    }
}
