﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Infra.Models;
using Inventory.Utils;
using Inventory.Views;
using Inventory.Views.Item;
using Models;
using Models.Exceptions;
using Models.Item;
using Models.Resps;
using Services;
using Services.Handlers.Exceptions;
using Services.Interfaces;
using System.Collections.ObjectModel;

namespace Inventory.ViewModels
{
    public partial class MainVM(IItemService itemBLL, IItemSituationService itemSituationBLL, IUserService userBLL) : VMBase
    {
        readonly Color BgButtonSelectedColor = Color.FromArgb("#29A0B1");

        UIItem selectedUiItem;

        public UIItem SelectedUiItem
        {
            get => selectedUiItem;
            set
            {
                if (selectedUiItem != value)
                {
                    selectedUiItem = value;

                    Shell.Current.GoToAsync($"{nameof(ItemDisplay)}?Id={selectedUiItem.Id}", true);
                }
            }
        }

        List<UIItem> ListAllItems;

        [ObservableProperty]
        ObservableCollection<UIItem> itemsObsList = [];

        [ObservableProperty]
        ObservableCollection<UIItemSituation> itemsSituationObsList = [];

        UIItemSituation SelectedUIItemsStatus { get; set; }

        [RelayCommand]
        public void ItemSituationSelectd(object e)
        {
            UIItemSituation? itemSituation = e as UIItemSituation;

            Color bgcolor = itemSituation.BackgoundColor;

            if (!bgcolor.Equals(BgButtonSelectedColor))
            {
                //if (SelectedUIItemsStatus.Count > 1)
                //{
                //ItemsSituationObsList.Where(x => x.Id == itemSituation.Id).First().BackgoundColor = Color.FromArgb("#919191");                    
                //SelectedUIItemsStatus.Remove(itemSituation);
                //}
                ItemsSituationObsList.Where(x => x.Id == SelectedUIItemsStatus.Id).ToList().ForEach(y => y.BackgoundColor = Color.FromArgb("#919191"));
                ItemsSituationObsList.Where(x => x.Id == itemSituation.Id).First().BackgoundColor = BgButtonSelectedColor;
                SelectedUIItemsStatus = itemSituation;
            }

            FilterItemsList();

            OnPropertyChanged(nameof(ItemsSituationObsList));
        }

        [RelayCommand]
        public async Task ItemAdd() => await Shell.Current.GoToAsync($"{nameof(ItemEdit)}");

        private void FilterItemsList()
        {
            IsBusy = true;

            ItemsObsList = [];

            foreach (UIItem? i in ListAllItems.Where(x => x.SituationId == SelectedUIItemsStatus.Id))//SelectedUIItemsStatus.Any(y => y.Id == x.SituationId)))
            {
                ItemsObsList.Add(i);
            }

            IsBusy = false;
        }

        [RelayCommand]
        public async Task Appearing()
        {
            if (((App)App.Current).Uid is null)
            {
                _ = Shell.Current.GoToAsync($"{nameof(SignIn)}");
                return;
            }

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                try
                {
                    Color backgoundColor;

                    List<ItemSituation> itemSituationList = [];

                    ServResp respItemSituation = await itemSituationBLL.GetItemSituation();

                    if (respItemSituation is not null && respItemSituation.Success)
                        itemSituationList = respItemSituation.Content as List<ItemSituation>;

                    List<Models.Item.Item>? respItems = await itemBLL.GetItemsAllAsync();

                    List<Models.Item.Item> itemList = [];
                    if (respItems is not null)
                    {
                        itemList = respItems;
                        itemList = [.. from item in itemList orderby item.CreatedAt descending select item];
                    }

                    if (itemSituationList is not null && itemSituationList.Count > 0)
                    {
                        ItemsSituationObsList = [];
                        string textSituationItem;

                        for (int i = 0; i < itemSituationList.Count; i++)
                        {
                            backgoundColor = itemSituationList[i].Sequence is 1 ? Color.FromArgb("#29A0B1") : Color.FromArgb("#919191");

                            textSituationItem = $"{itemSituationList[i].Name} ({itemList.Where(x => x.Situation.Id == itemSituationList[i].Id).Count()})";

                            ItemsSituationObsList.Add(new UIItemSituation() { Id = itemSituationList[i].Id.Value, Name = textSituationItem, BackgoundColor = backgoundColor });
                        }

                        if (SelectedUIItemsStatus is null)
                        {
                            OnPropertyChanged(nameof(ItemsSituationObsList));

                            SelectedUIItemsStatus = ItemsSituationObsList.First();
                        }

                        //AcquisitionTypeList = new ObservableCollection<UIAcquisitionType>();

                        //foreach (var _acquisitionType in UIModels.UIAcquisitionTypeList.UIAcquisitionTypes)
                        //{
                        //    AcquisitionTypeList.Add(_acquisitionType);
                        //}


                        //FilterItemsList();

                        ItemsObsList = [];
                        ListAllItems = [];

                        string IconUniCode;
                        if (itemList is not null)
                            foreach (Models.Item.Item item in itemList)
                            {
                                string categoryAndSubCategory = "";

                                categoryAndSubCategory = item.Category.Name;

                                if (item.Category.SubCategory is not null)
                                    categoryAndSubCategory += "/" + item.Category.SubCategory.Name;

                                IconUniCode = item.Category.SubCategory is null || item.Category.SubCategory.IconName is null
                                    ? Icons.Tag
                                    : SubCategoryIconsList.GetIconCode(item.Category.SubCategory.IconName);

                                UIItem uIItem = new()
                                {
                                    Id = item.Id.Value,
                                    Name = item.Name,
                                    CategoryAndSubCategory = categoryAndSubCategory,
                                    CategoryColor = Color.FromArgb(item.Category.Color),
                                    SituationId = item.Situation.Id.Value,
                                    SubCategoryIcon = IconUniCode,
                                };

                                ListAllItems.Add(uIItem);

                                if (item.Situation.Id == SelectedUIItemsStatus.Id)
                                    ItemsObsList.Add(uIItem);

                                //if (SelectedUIItemsStatus.Exists(x => x.Id == item.Situation))
                                //    ItemsObsList.Add(uIItem);
                            }

                        IsBusy = false;
                    }
                }
                catch (ServerOffException)
                {
                    await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Não foi possível se conectar com o servidor", null, "Ok");
                }
                catch (SignInFailException ex)
                {
                    userBLL.RemoveUserLocal();

                    await Shell.Current.GoToAsync($"//{nameof(SignIn)}");
                }
                catch (Exception ex) { throw ex; }
            }
        }
    }
}
