namespace Inventory.Infra.Models
{
    public class UIItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string CategoryAndSubCategory { get; set; }

        public string CategoryName { get; set; }

        public Color CategoryColor { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public string SubCategoryIcon { get; set; }

        public int SituationId { get; set; }

        public string TechnicalDescription { get; set; }

        public string Comments { get; set; }
    }
}
