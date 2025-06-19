using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Infra.Models;
using Inventory.Views.Item.Selectors;
using Models.Resps;
using Services.Interfaces;
using System.Collections.ObjectModel;

namespace Inventory.ViewModels.Item.Selectors
{
    public partial class CategorySelectorVM(ICategoryService categoryService) : VMBase
    {
        [ObservableProperty]
        private ObservableCollection<UICategory> categoriesObsList = [];

        private UICategory selectedCategory;

        public UICategory SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;

                    Shell.Current.GoToAsync($"{nameof(SubCategorySelector)}", true, new Dictionary<string, object> { { "Category", selectedCategory } });
                    SetProperty(ref selectedCategory, value);
                }
            }
        }


        [RelayCommand]
        private async Task Appearing()
        {
            CategoriesObsList = [];
            List<Models.DTO.CategoryDTO> Categorylist = [];

            ServResp resp = await categoryService.GetCategoriesWithSubCategoriesAsync();

            if (resp is not null && resp.Success)
                Categorylist = resp.Content as List<Models.DTO.CategoryDTO>;

            CategoriesObsList.Add(new UICategory() { Id = -1, Name = "[Sem Categoria]", Color = Color.FromArgb("#2F9300"), HaveSubcategories = false });

            if (Categorylist != null && Categorylist.Count > 0)
                foreach (Models.DTO.CategoryDTO i in Categorylist)
                    CategoriesObsList.Add(new UICategory() { Id = i.Id.Value, Name = i.Name, Color = Color.FromArgb(i.Color), HaveSubcategories = i.SubCategories.Count > 0, SubCategories = i.SubCategories });

        }
    }
}
