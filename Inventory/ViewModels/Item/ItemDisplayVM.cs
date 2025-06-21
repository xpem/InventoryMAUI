using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Infra.Models;
using Inventory.Views.Item;
using Models.Item;
using Models.Item.Files;
using Models.Resps;
using Services.Interfaces;
using System.Collections.ObjectModel;

namespace Inventory.ViewModels.Item
{
    public partial class ItemDisplayVM(IItemService itemService) : VMBase, IQueryAttributable
    {
        #region fields

        int ItemId { get; set; }

        [ObservableProperty]
        string name, description, categoryAndSubCategory, acquisitionStore, acquisitionTypeName, commentary, situation, resaleValue, acquisitionDate, withdrawalDate, acquisitionValue;

        [ObservableProperty]
        bool resaleSituation, withdrawalDateIsVisible, crvwIsVisible;

        [ObservableProperty]
        public ObservableCollection<UIImagePath> imagePathsObsCol = [];

        #endregion

        [RelayCommand]
        public async Task Edit() => Shell.Current.GoToAsync($"{nameof(ItemEdit)}?Id={ItemId}", true);

        [RelayCommand]
        public async Task DelItem() => await DeleteItem();

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            IsBusy = true;
            if (query.ContainsKey("Id") && query.TryGetValue("Id", out object itemId))
            {
                ItemId = Convert.ToInt32(itemId);
                Models.Item.Item item;

                ServResp resp = await itemService.GetItemByIdAsync(ItemId.ToString());

                if (resp is not null && resp.Success)
                {
                    item = resp.Content as Models.Item.Item;
                    Name = item.Name;
                    //AcquisitionValue = item.PurchaseValue.ToString();

                    string _categoryAndSubCategory = item.Category.Name;

                    if (item.Category.SubCategory is not null)
                    {
                        _categoryAndSubCategory += "/" + item.Category.SubCategory.Name;
                    }

                    Situation = item.Situation.Name;

                    CategoryAndSubCategory = _categoryAndSubCategory;
                    Description = item.TechnicalDescription;
                    WithdrawalDateIsVisible = ResaleSituation = false;

                    WithdrawalDate = string.Format("dd/MM/yyyy", item.WithdrawalDate != null ? item.WithdrawalDate.Value : DateTime.Now);
                    // AcquisitionType = item.AcquisitionType;

                    if (item.Situation.Id == OutSituationsIds.ResaleStatusId)
                    {
                        ResaleSituation = true;
                        ResaleValue = item.ResaleValue.ToString();
                    }

                    if (OutSituationsIds.OutSituations.Contains(item.Situation.Id.Value))
                    {
                        WithdrawalDateIsVisible = true;
                        WithdrawalDate = item.WithdrawalDate.Value.ToString("d");
                    }

                    AcquisitionTypeName = item.AcquisitionType.Name;
                    AcquisitionDate = item.AcquisitionDate.ToString("d");
                    AcquisitionValue = item.PurchaseValue.ToString();
                    AcquisitionStore = item.PurchaseStore;

                    Commentary = item.Comment;

                    ImagePathsObsCol = [];

                    ItemFilesToUpload listImagePaths = await itemService.GetItemImages(ItemId, item.Image1, item.Image2);

                    if (listImagePaths != null)
                    {
                        if (listImagePaths.Image1 != null)
                            ImagePathsObsCol.Add(new UIImagePath(listImagePaths.Image1.ImageFilePath, listImagePaths.Image1.FileName, item.Image1));

                        if (listImagePaths.Image2 != null)
                            ImagePathsObsCol.Add(new UIImagePath(listImagePaths.Image2.ImageFilePath, listImagePaths.Image2.FileName, item.Image2));
                    }

                    if (ImagePathsObsCol.Count > 0)
                    {
                        OnPropertyChanged(nameof(ImagePathsObsCol));
                        CrvwIsVisible = true;
                    }
                    else { CrvwIsVisible = false; }
                }
            }
            IsBusy = false;
        }

        private async Task DeleteItem()
        {
            if (await Application.Current.Windows[0].Page.DisplayAlert("Confirmação", "Deseja excluir este Item?", "Sim", "Cancelar"))
            {
                IsBusy = true;

                await itemService.DelItemAsync(ItemId);

                IsBusy = false;

                if (!await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Item excluído!", null, "Ok"))
                    await Shell.Current.GoToAsync("..");
            }
        }

    }
}
