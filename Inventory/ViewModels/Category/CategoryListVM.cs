using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Infra.Models;
using Inventory.Views.Category;
using Models.Resps;
using Services.Interfaces;
using System.Collections.ObjectModel;

namespace Inventory.ViewModels.Category
{
    public partial class CategoryListVM(ICategoryService categoryService) : VMBase
    {

        [ObservableProperty]

        ObservableCollection<UICategory> categories;

        UICategory selectedUICategory;

        public UICategory SelectedUICategory
        {
            get => selectedUICategory;
            set
            {
                if (selectedUICategory != value)
                {
                    selectedUICategory = value;

                    Shell.Current.GoToAsync($"{nameof(CategoryDisplay)}?Id={selectedUICategory.Id}", true);
                }
            }
        }

        [RelayCommand]
        private async Task CategoryAdd() => await Shell.Current.GoToAsync($"{nameof(CategoryEdit)}");

        [RelayCommand]
        private async Task Appearing()
        {
            IsBusy = true;
            Categories = [];

            List<Models.DTO.CategoryDTO> list = [];

            ServResp resp = await categoryService.GetCategoriesAsync();

            if (resp is not null && resp.Success)
                list = resp.Content as List<Models.DTO.CategoryDTO>;

            if (list != null && list.Count > 0)
                foreach (var i in list)
                {
                    Categories.Add(new UICategory() { Id = i.Id.Value, Name = i.Name, Color = Color.FromArgb(i.Color) });
                }

            IsBusy = false;
        }
    }
}

