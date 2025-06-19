using CommunityToolkit.Mvvm.ComponentModel;
using Models.DTO;

namespace Inventory.Infra.Models
{
    public partial class UICategory : ObservableObject
    {
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        Color backgoundColor;

        public Color Color { get; set; }

        public bool HaveSubcategories { get; set; }

        public List<SubCategoryDTO> SubCategories { get; set; }
    }
}
