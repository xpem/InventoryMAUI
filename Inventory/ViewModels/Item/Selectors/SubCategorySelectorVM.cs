using CommunityToolkit.Mvvm.ComponentModel;
using Inventory.Infra.Models;
using Inventory.Utils;
using Models.DTO;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Inventory.ViewModels.Item.Selectors
{
    public partial class SubCategorySelectorVM : VMBase, IQueryAttributable
    {
        [ObservableProperty]
        int categoryId;

        [ObservableProperty]
        Color categoryColor;

        [ObservableProperty]
        string categoryName;

        public ObservableCollection<UISubCategory> SubCategoryObsList { get; set; }

        public UICategory Category { get; set; }

        private UISubCategory selectedSubCategory;

        public UISubCategory SelectedSubCategory
        {
            get => selectedSubCategory;
            set
            {
                if (selectedSubCategory != value)
                {
                    selectedSubCategory = value;

                    var subCategoryObj = Category.SubCategories.FirstOrDefault(c => c.Id == selectedSubCategory.Id);

                    List<SubCategoryDTO> subCategories = [subCategoryObj];


                    Shell.Current.GoToAsync($"../..", true, new Dictionary<string, object> { { "SelectedCategory", Category } });

                    SetProperty(ref selectedSubCategory, value);
                }
            }
        }


        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Category", out object value))
            {
                Category = value as UICategory;

                CategoryName = Category.Name;
                CategoryColor = Category.Color;
                CategoryId = Category.Id;

                SubCategoryObsList = [];

                if (Category.SubCategories != null && Category.SubCategories.Count > 0)
                    foreach (SubCategoryDTO subCategory in Category.SubCategories)
                        SubCategoryObsList.Add(
                            new UISubCategory()
                            {
                                Id = subCategory.Id,
                                Icon = SubCategoryIconsList.GetIconCode(subCategory.IconName),
                                Name = subCategory.Name,
                                SystemDefault = subCategory.SystemDefault
                            });

                OnPropertyChanged(nameof(SubCategoryObsList));
            }
        }

        public ICommand SelectCategoryCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"../..", true, new Dictionary<string, object> { { "SelectedCategory", new CategoryDTO() { Id = CategoryId, Name = categoryName } } });
        });

    }
}
