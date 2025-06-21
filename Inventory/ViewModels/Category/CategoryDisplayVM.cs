using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Infra.Models;
using Inventory.Utils;
using Microsoft.EntityFrameworkCore.Metadata;
using Models.Resps;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.ViewModels.Category
{
    public partial class CategoryDisplayVM(ICategoryService categoryService, ISubCategoryService subCategoryService) : VMBase, IQueryAttributable
    {
        public int CurrentPage { get; set; }

        int Id;

        [ObservableProperty]
        Color categoryColor;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        bool systemDefault;


        public ObservableCollection<UISubCategory> SubCategoryObsCol { get; set; } = [];

        [RelayCommand]
        private async Task CategoryEdit() => await Shell.Current.GoToAsync($"{nameof(Views.Category.CategoryEdit)}?Id={Id}", true);

        [RelayCommand]
        private async Task AddSubCategory() => await Shell.Current.GoToAsync($"{nameof(Views.Category.SubCategory.SubCategoryEdit)}", true, new Dictionary<string, object> { { "Category", (new Models.DTO.CategoryDTO() { Id = Id, Name = Name }) } });

        [RelayCommand]
        private async Task SubCategoryEdit(int id) => await Shell.Current.GoToAsync($"{nameof(Views.Category.SubCategory.SubCategoryEdit)}?Id={id}", true, new Dictionary<string, object> { { "Category", (new Models.DTO.CategoryDTO() { Id = Id, Name = Name }) } });

        [RelayCommand]
        private async Task DeleteCategory()
        {
            if (SubCategoryObsCol.Count > 0)
            {
                _ = Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Não é possivel excluir uma categoria que tenha subcategorias relacionadas", null, "Ok");
            }
            else
            {
                if (await Application.Current.Windows[0].Page.DisplayAlert("Confirmação", "Deseja excluir esta Categoria?", "Sim", "Cancelar"))
                {
                    bool success = false;
                    string message = null;
                    ServResp resp = await categoryService.DelCategoryAsync(Id);

                    if (resp.Success)
                    {
                        success = resp.Success;
                        message = resp.Content as string;
                    }

                    if (success)
                    {
                        if (!await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Categoria excluída!", null, "Ok"))
                            await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        if (message != null)
                            await Application.Current.Windows[0].Page.DisplayAlert("Aviso", message, null, "Ok");
                        else
                            throw new Exception("Houve um erro ao tentar excluir A Categoria");
                    }
                }
            }
        }

        [RelayCommand]
        private async Task LoadMore()
        {
            CurrentPage++;
            await LoadSubCategories(CurrentPage);
        }

        [RelayCommand]
        private async Task DeleteSubCategory(int id)
        {
            if (await Application.Current.Windows[0].Page.DisplayAlert("Confirmação", "Deseja excluir esta Sub Categoria?", "Sim", "Cancelar"))
            {
                bool success = false;
                string? message = null;
                var resp = await subCategoryService.DelSubCategory(Convert.ToInt32(id));

                if (resp.Success)
                {
                    success = true;
                    message = "Sub Categoria Excluída!";
                }

                if (success)
                {
                    if (!await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Sub Categoria excluída!", null, "Ok"))
                    {
                        UISubCategory sub = SubCategoryObsCol.Where(x => x.Id == Convert.ToInt32(id)).First();
                        SubCategoryObsCol.Remove(sub);
                        OnPropertyChanged(nameof(SubCategoryObsCol));
                        //await Shell.Current.GoToAsync("..");
                    }
                }
                else
                {
                    if (message != null)
                        await Application.Current.Windows[0].Page.DisplayAlert("Aviso", message, null, "Ok");
                    else
                        throw new Exception("Houve um erro ao tentar excluir a Sub Categoria");
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Id = Convert.ToInt32(query["Id"]);
        }

        [RelayCommand]
        private async Task Appearing()
        {
            Models.DTO.CategoryDTO? category = null;

            ServResp resp = await categoryService.GetCategoryByIdAsync(Id.ToString());

            if (resp.Success)
                category = resp.Content as Models.DTO.CategoryDTO;

            //Category no longer exists in the db
            if (category is null) await Shell.Current.GoToAsync("..");

            CategoryColor = Color.FromArgb(category.Color);
            Name = category.Name;
            SystemDefault = category.SystemDefault.Value;
            SubCategoryObsCol = [];

            CurrentPage = 1;

            _ = LoadSubCategories(CurrentPage);

            OnPropertyChanged(nameof(SubCategoryObsCol));
        }

        private async Task LoadSubCategories(int pageNumber)
        {
            IsBusy = true;

            List<Models.DTO.SubCategoryDTO> subCategoryList = await subCategoryService.GetByCategoryIdAsync(((App)App.Current).Uid.Value, pageNumber, Id);

            if (subCategoryList != null && subCategoryList.Count > 0)
                foreach (var subCategory in subCategoryList)
                    SubCategoryObsCol.Add(new UISubCategory() { Id = subCategory.Id, Icon = SubCategoryIconsList.GetIconCode(subCategory.IconName), Name = subCategory.Name, SystemDefault = !subCategory.SystemDefault.Value });

            IsBusy = false;
        }
    }
}
