using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Models.Resps;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.ViewModels.Category
{
    public partial class CategoryEditVM(ICategoryService categoryService) : VMBase,IQueryAttributable
    {
        int Id;

        [ObservableProperty]
        Color categoryColor;


        [ObservableProperty]
        string name, btnInsertText, btnInsertIcon;

        [ObservableProperty]

        bool colorPickerVisible, buttonColorVisible, btnInsertIsEnabled = true;

        [RelayCommand]
        private async Task ShowColorPicker()
        {
            ColorPickerVisible = true;
            ButtonColorVisible = false;
        }

        [RelayCommand]

        private async Task DefineColor(string color)
        {
            CategoryColor = Color.FromArgb(color as string);
            ColorPickerVisible = false;
            ButtonColorVisible = true;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            IsBusy = true;
            ColorPickerVisible = false;
            ButtonColorVisible = true;

            if (query.Count > 0)
            {
                Id = Convert.ToInt32(query["Id"]);
                Models.DTO.CategoryDTO category = null;

                ServResp resp = await categoryService.GetCategoryByIdAsync(Id.ToString());

                if (resp.Success)
                    category = resp.Content as Models.DTO.CategoryDTO;

                CategoryColor = Color.FromArgb(category.Color);

                Name = category.Name;

                BtnInsertIcon = Icons.Pen;
                BtnInsertText = "Atualizar";
            }
            else
            {
                CategoryColor = Color.FromArgb("#a3e4d7");

                Id = 0;
                Name = "";
                BtnInsertIcon = Icons.Plus;
                BtnInsertText = "Cadastrar";
            }

            IsBusy = false;
        }

        [RelayCommand]
        private async Task Insert() =>  await InsertCategory();

        private Page _Page => Application.Current.Windows[0].Page;

        public async Task InsertCategory()
        {
            try
            {
                if (await VerifyFields())
                {
                    BtnInsertIsEnabled = false;

                    Models.DTO.CategoryDTO category = new()
                    {
                        Name = Name,
                        Color = CategoryColor.ToArgbHex(),
                    };

                    string message = "";

                    if (Id > 0)
                    {
                        category.Id = Id;

                        ServResp resp = await categoryService.AltCategoryAsync(category);

                        if (resp.Success)
                            message = "Categoria Atualizada!";
                    }
                    else
                    {
                        ServResp resp = await categoryService.AddCategoryAsync(category);

                        if (resp.Success)
                            message = "Categoria Adicionada!";
                    }

                    bool resposta = await _Page.DisplayAlert("Aviso", message, null, "Ok");

                    if (!resposta)
                        await Shell.Current.GoToAsync("..");

                    BtnInsertIsEnabled = true;
                }

            }
            catch (Exception ex) { throw ex; }
        }

        private async Task<bool> VerifyFields()
        {
            bool valid = true;

            if (string.IsNullOrEmpty(Name))
            {
                valid = false;
                _ = await _Page.DisplayAlert("Aviso", "Digite um Nome válido", null, "Ok");
            }

            return valid;
        }
    }
}
