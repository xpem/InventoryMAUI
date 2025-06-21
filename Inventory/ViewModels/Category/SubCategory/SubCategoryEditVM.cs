using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Utils;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.ViewModels.Category.SubCategory
{
    public partial class SubCategoryEditVM(ISubCategoryService subCategoryService) : VMBase,IQueryAttributable
    {
        int CategoryId, Id;

        [ObservableProperty]
        string categoryName, name, icon;

        [ObservableProperty]
        bool iconPickerVisible;

        [ObservableProperty]
        bool buttonIconVisible, btnInsertIsEnabled = true;


        [ObservableProperty]
        string btnConfirmationIcon, btnConfirmationText;

        [RelayCommand]
        private async Task ShowIconPicker()
        {
            IconPickerVisible = true;
            ButtonIconVisible = false;
            Icon = Icons.Tag;
        }


        [RelayCommand]
        private async Task DefineIcon(string iconDefinition)
        {
            Icon = iconDefinition;
            IconPickerVisible = false;
            ButtonIconVisible = true;
        }

        [RelayCommand]
        private async Task Add()=> await InsertSubCategory();

        public async Task InsertSubCategory()
        {
            try
            {
                if (await Validate())
                {
                    BtnInsertIsEnabled = false;

                    Models.DTO.SubCategoryDTO subCategory = new()
                    {
                        Name = Name,
                        IconName = SubCategoryIconsList.GetIconName(icon),
                        CategoryId = CategoryId
                    };

                    string message = "";

                    if (Id > 0)
                    {
                        subCategory.Id = Id;

                        var resp = await subCategoryService.UpdateAsync(((App)Application.Current).Uid.Value, IsOn, subCategory);

                        if (resp.Success)
                            message = "Sub Categoria Adicionada!";
                    }
                    else
                    {
                        var resp = await subCategoryService.CreateAsync(((App)Application.Current).Uid.Value, IsOn, subCategory);

                        if (resp.Success)
                            message = "Sub Categoria Adicionada!";
                    }
                    bool resposta = await Application.Current.MainPage.DisplayAlert("Aviso", message, null, "Ok");

                    if (!resposta)
                        await Shell.Current.GoToAsync("..");


                    BtnInsertIsEnabled = true;
                }

            }
            catch (Exception ex) { throw ex; }
        }

        private async Task<bool> Validate()
        {
            bool valid = true;

            if (string.IsNullOrEmpty(Name))
            {
                valid = false;
                _ = await Application.Current.MainPage.DisplayAlert("Aviso", "Digite um Nome válido", null, "Ok");
            }

            return valid;
        }


        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            IsBusy = true;
            IconPickerVisible = false;
            ButtonIconVisible = true;

            if (query.TryGetValue("Id", out object _id))
                Id = Convert.ToInt32(_id);

            if (query.TryGetValue("Category", out object value))
            {
                var category = value as Models.DTO.CategoryDTO;
                CategoryName = category.Name;
                CategoryId = category.Id.Value;
            }

            if (Id != 0)
            {
                Models.DTO.SubCategoryDTO subCategory;

                var resp = await subCategoryService.GetByIdAsync(Id);

                subCategory = resp as Models.DTO.SubCategoryDTO;

                Name = subCategory.Name;
                CategoryId = subCategory.CategoryId;
                Icon = SubCategoryIconsList.GetIconCode(subCategory.IconName);

                BtnConfirmationText = "Alterar";
                BtnConfirmationIcon = Icons.Pen;
            }
            else
            {
                BtnConfirmationText = "Cadastrar";
                BtnConfirmationIcon = Icons.Plus;
            }

            Icon ??= Icons.Tag;
            IsBusy = false;
        }
    }
}
