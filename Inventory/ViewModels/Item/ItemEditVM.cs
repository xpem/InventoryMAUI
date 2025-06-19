using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Infra.Models;
using Models;
using Models.DTO;
using Models.Item;
using Models.Item.Files;
using Models.Resps;
using Services;
using Services.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Inventory.ViewModels.Item
{
    public partial class ItemEditVM(IItemService itemService, IItemSituationService itemSituationService, IAcquisitionTypeService acquisitionTypeService) : VMBase, IQueryAttributable
    {
        int ItemId { get; set; }

        int CategoryId { get; set; }

        int? SubCategoryId { get; set; }

        const int resaleStatusId = 5;

        readonly int[] outSituations = [4, 5, 3, 7];

        public enum MediaPickerType { pick, capture }

        #region fields and components behaviors

        [ObservableProperty]
        string name, description, categoryName, acquisitionStore, commentary,
            acquisitionValue, resaleValue, btnInsertText, btnInsertIcon;

        [ObservableProperty]
        bool btnPickItemImageIsEnabled, btnInsertIsEnabled = true, btnDeleteIsVisible, stlResaleValueIsVisible, stlWithdrawalDateIsVisible, crvwIsVisible, vSLAddImageIsVisible = true;

        [ObservableProperty]
        DateTime acquisitionDate, withdrawalDate;

        [ObservableProperty]
        int pkrAcquisitionTypeSelectedIndex;

        int pkrItemSituationSelectedIndex;

        public int PkrItemSituationSelectedIndex
        {
            get => pkrItemSituationSelectedIndex;
            set
            {
                if (pkrItemSituationSelectedIndex != value)
                {
                    pkrItemSituationSelectedIndex = value;
                    if (pkrItemSituationSelectedIndex > 0)
                    {
                        if (ItemsSituationObsList[pkrItemSituationSelectedIndex].Id == resaleStatusId)
                        {
                            StlResaleValueIsVisible = true;
                            ResaleValue = "0";
                            OnPropertyChanged(nameof(ResaleValue));
                        }
                        else StlResaleValueIsVisible = false;

                        if (outSituations.Contains(ItemsSituationObsList[pkrItemSituationSelectedIndex].Id))
                        {
                            StlWithdrawalDateIsVisible = true;
                            WithdrawalDate = DateTime.Now;
                            OnPropertyChanged(nameof(ResaleValue));
                        }
                        else StlWithdrawalDateIsVisible = false;
                    }
                }
                OnPropertyChanged(nameof(PkrItemSituationSelectedIndex));
            }
        }
        FixedIndexesImagePaths ImagePaths { get; set; } = new();

        [ObservableProperty]
        ObservableCollection<UIImagePath> imagePathsObsCol;

        [ObservableProperty]
        UIItemSituation itemSituation;

        [ObservableProperty]
        private ObservableCollection<UIItemSituation> itemsSituationObsList;

        [ObservableProperty]
        public ObservableCollection<UIAcquisitionType> acquisitionTypeObsList;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task CategorySelector() => await Shell.Current.GoToAsync($"{nameof(Views.Item.Selectors.CategorySelector)}", true);

        [RelayCommand]
        public async Task AddItem() => await AltItem();

        [RelayCommand]
        public async Task PickItemImage() => await TakeItemImage(MediaPickerType.pick);

        [RelayCommand]
        public async Task CaptureItemImage() => await TakeItemImage(MediaPickerType.capture);

        [RelayCommand]
        public async Task DelItemImage(object e)
        {

            UIImagePath itemImage;
            Guid imageId = Guid.Parse(e as string);

            if (ImagePaths.Image1 is not null && ImagePaths.Image1.Id == imageId)
            {
                itemImage = ImagePaths.Image1;

                ImagePaths.Image1 = null;

                BuildImagePathsObsColAsync();

                ItemEditVM.DeleteLocalFile(itemImage.ImageFilePath);

                if (itemImage.ExternalFileName is not null)
                {
                    _ = itemService.DelItemImageAsync(ItemId, itemImage.ExternalFileName);
                }
            }
            else if (ImagePaths.Image2 is not null)
            {
                itemImage = ImagePaths.Image2;
                ImagePaths.Image2 = null;

                BuildImagePathsObsColAsync();
                ItemEditVM.DeleteLocalFile(itemImage.ImageFilePath);

                if (itemImage.ExternalFileName is not null)
                {
                    _ = itemService.DelItemImageAsync(ItemId, itemImage.ExternalFileName);
                }
            }
        }


        #endregion

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            //backing of Category Selection Function
            if (query.ContainsKey("SelectedCategory") && query.TryGetValue("SelectedCategory", out object selectedCategory))
            {
               UICategory modelSelectedCategory = selectedCategory as UICategory;
                CategoryId = modelSelectedCategory.Id;

                if (modelSelectedCategory?.SubCategories?.Count > 0)
                {
                    CategoryName = modelSelectedCategory.Name + "/" + modelSelectedCategory.SubCategories[0].Name;
                    SubCategoryId = modelSelectedCategory.SubCategories[0].Id.Value;
                }
                else
                {
                    CategoryName = modelSelectedCategory.Name;
                }
            }
            else
            {
                IsBusy = true;
                DateTime itemAcquisitionDate = DateTime.Now;

                ItemsSituationObsList = [];
                List<ItemSituation> itemSituationList = [];

                ServResp? respItemSituationList = await itemSituationService.GetItemSituation();

                if (respItemSituationList is not null && respItemSituationList.Success)
                    itemSituationList = respItemSituationList.Content as List<ItemSituation>;

                ItemsSituationObsList.Add(new UIItemSituation() { Id = -1, Name = "Selecione" });

                foreach (ItemSituation itemSituation in itemSituationList)
                    ItemsSituationObsList.Add(new UIItemSituation() { Id = itemSituation.Id.Value, Name = itemSituation.Name });

                OnPropertyChanged(nameof(ItemsSituationObsList));

                AcquisitionTypeObsList = [];

                List<AcquisitionType> acquisitionTypeList = [];

                ServResp? respAcquisitionTypeList = await acquisitionTypeService.GetAcquisitionType();

                if (respAcquisitionTypeList is not null && respAcquisitionTypeList.Success)
                    acquisitionTypeList = respAcquisitionTypeList.Content as List<AcquisitionType>;

                AcquisitionTypeObsList.Add(new UIAcquisitionType() { Id = -1, Name = "Selecione" });

                foreach (AcquisitionType acquisitionType in acquisitionTypeList)
                    AcquisitionTypeObsList.Add(new UIAcquisitionType() { Id = acquisitionType.Id.Value, Name = acquisitionType.Name });

                OnPropertyChanged(nameof(AcquisitionTypeObsList));

                if (query.ContainsKey("Id") && query.TryGetValue("Id", out object itemId))
                {
                    ItemId = Convert.ToInt32(itemId);
                    Models.Item.Item item;

                    ServResp resp = await itemService.GetItemByIdAsync(ItemId.ToString());

                    if (resp is not null && resp.Success)
                    {
                        item = resp.Content as Models.Item.Item;

                        itemAcquisitionDate = item.AcquisitionDate;
                        Name = item.Name;
                        AcquisitionValue = item.PurchaseValue.ToString();

                        string categoryAndSubCategory = item.Category.Name;
                        CategoryId = item.Category.Id.Value;

                        if (item.Category.SubCategory is not null)
                        {
                            categoryAndSubCategory += "/" + item.Category.SubCategory.Name;
                            SubCategoryId = item.Category.SubCategory.Id.Value;
                        }

                        CategoryName = categoryAndSubCategory;
                        Description = item.TechnicalDescription;
                        Commentary = item.Comment;

                        PkrItemSituationSelectedIndex = ItemsSituationObsList.IndexOf(ItemsSituationObsList.Where(s => s.Id == item.Situation.Id).FirstOrDefault());
                        PkrAcquisitionTypeSelectedIndex = AcquisitionTypeObsList.IndexOf(AcquisitionTypeObsList.Where(s => s.Id == item.AcquisitionType.Id).FirstOrDefault());
                        ResaleValue = item.ResaleValue.ToString();
                        WithdrawalDate = item.WithdrawalDate != null ? item.WithdrawalDate.Value : DateTime.Now;
                        AcquisitionStore = item.PurchaseStore;

                        ImagePathsObsCol = [];

                        ItemFilesToUpload responseItemImages = await itemService.GetItemImages(ItemId, item.Image1, item.Image2);

                        if (responseItemImages != null)
                        {
                            if (responseItemImages.Image1 != null)
                            {
                                string image1ExternalId = item.Image1;
                                ImagePaths.Image1 = new(responseItemImages.Image1.ImageFilePath, fileName: responseItemImages.Image1.FileName, image1ExternalId);
                            }

                            if (responseItemImages.Image2 != null)
                            {
                                string image2ExternalId = item.Image2;
                                ImagePaths.Image2 = new(responseItemImages.Image2.ImageFilePath, fileName: responseItemImages.Image2.FileName, image2ExternalId);
                            }
                        }

                        BuildImagePathsObsColAsync();
                    }

                    BtnInsertIcon = Icons.Pen;
                    BtnInsertText = "Atualizar";
                    BtnDeleteIsVisible = true;
                    BtnPickItemImageIsEnabled = true;
                }
                else
                {
                    CategoryName = "Selecione";
                    Name = Description = string.Empty;
                    AcquisitionValue = "0";

                    BtnInsertIcon = Icons.Plus;
                    BtnInsertText = "Cadastrar";
                    BtnDeleteIsVisible = false;
                    BtnPickItemImageIsEnabled = true;

                    PkrItemSituationSelectedIndex = 0;

                    PkrAcquisitionTypeSelectedIndex = 0;
                }

                AcquisitionDate = itemAcquisitionDate;
                IsBusy = false;
            }
        }

        private async Task BuildImagePathsObsColAsync()
        {
            ImagePathsObsCol = [];

            if (ImagePaths != null)
            {
                if (ImagePaths.Image1 != null)
                    ImagePathsObsCol.Add(ImagePaths.Image1);

                if (ImagePaths.Image2 != null)
                    ImagePathsObsCol.Add(ImagePaths.Image2);
            }

            if (ImagePathsObsCol.Count > 0)
            {
                CrvwIsVisible = true;

                BtnPickItemImageIsEnabled = ImagePathsObsCol.Count != 2;
            }
            else
            {
                CrvwIsVisible = false;
                BtnPickItemImageIsEnabled = true;
            }
        }

        private static decimal CurrencyValueParse(string currencyValue) =>
            decimal.Parse(currencyValue.Replace(".", ""), NumberStyles.Number, new NumberFormatInfo() { NumberDecimalSeparator = "," });

        private async Task AltItem()
        {
            try
            {
                if (await Validate())
                {
                    BtnInsertIsEnabled = false;

                    decimal decAquisitionValue = CurrencyValueParse(AcquisitionValue);
                    decimal decResaleValue = ItemsSituationObsList[pkrItemSituationSelectedIndex].Id == resaleStatusId ? CurrencyValueParse(ResaleValue) : 0;

                    Models.Item.Item item = new()
                    {
                        Name = Name.Trim(),
                        AcquisitionDate = new DateTime(AcquisitionDate.Year, AcquisitionDate.Month, AcquisitionDate.Day).Date,
                        AcquisitionType = new AcquisitionType() { Id = AcquisitionTypeObsList[pkrAcquisitionTypeSelectedIndex].Id },
                        Comment = Commentary?.Trim(),
                        PurchaseStore = AcquisitionStore?.Trim(),
                        PurchaseValue = decAquisitionValue,
                        Situation = new ItemSituation() { Id = ItemsSituationObsList[pkrItemSituationSelectedIndex].Id },
                        ResaleValue = StlResaleValueIsVisible ? decResaleValue : null,
                        TechnicalDescription = Description.Trim(),
                        Category = new Models.DTO.CategoryDTO() { Id = CategoryId, SubCategory = SubCategoryId is not null ? new SubCategoryDTO() { Id = SubCategoryId.Value } : null },
                        WithdrawalDate = StlWithdrawalDateIsVisible ? WithdrawalDate : null
                    };

                    string message = "";
                    ServResp resp;

                    if (ItemId > 0)
                    {
                        item.Id = ItemId;

                        resp = await itemService.AltItemAsync(item);
                    }
                    else
                        resp = await itemService.AddItemAsync(item);

                    if (resp.Success)
                    {
                        if (ItemId > 0)
                            message = "Item Atualizada!";
                        else
                        {
                            message = "Item Adicionado!";

                            if (resp.Content is Models.Item.Item)
                            {
                                Models.Item.Item AddedItem = resp.Content as Models.Item.Item;
                                ItemId = AddedItem.Id.Value;
                            }

                            if (ItemId == 0) throw new Exception("Id do item zerado após criação de item");
                        }

                        ItemFilesToUpload itemFilesToUpload = new();

                        if (ImagePaths.Image1 != null || ImagePaths.Image2 != null)
                        {
                            if (ImagePaths.Image1 != null)
                                itemFilesToUpload.Image1 = new() { FileName = ImagePaths.Image1.FileName, FileId = 1, ImageFilePath = ImagePaths.Image1.ImageFilePath };

                            if (ImagePaths.Image2 != null)
                                itemFilesToUpload.Image2 = new() { FileName = ImagePaths.Image2.FileName, FileId = 2, ImageFilePath = ImagePaths.Image2.ImageFilePath };

                            ServResp? respAddItemImages = await itemService.AddItemImageAsync(ItemId, itemFilesToUpload);

                            if (respAddItemImages is not null && respAddItemImages.Success)
                            {
                                ItemFileNames? itemFileNames = respAddItemImages.Content as Models.Item.Files.ItemFileNames;
                            }
                        }
                    }
                    else if (resp.Content != null)
                        message = resp.Content as string;

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
                valid = false;

            if (!valid) { await Application.Current.MainPage.DisplayAlert("Aviso", "preencha com um nome válido", null, "Ok"); }
            else
            {
                if (PkrItemSituationSelectedIndex is 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Selecione uma situação válida", null, "Ok");
                    valid = false;
                }
                if (pkrAcquisitionTypeSelectedIndex is 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Selecione um tipo de aquisição válida", null, "Ok");
                    valid = false;
                }
            }

            return valid;
        }

        private static void DeleteLocalFile(string imageFilePath) => File.Delete(Path.Combine(imageFilePath));

        public async Task TakeItemImage(MediaPickerType mediaPickerType)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = mediaPickerType is MediaPickerType.pick
                    ? await MediaPicker.Default.PickPhotoAsync()
                    : await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    string tempFileName = new Guid().ToString();
                    int fileIdx = 1;
                    if (ImagePathsObsCol is not null && ImagePathsObsCol.Count == 1 && ImagePaths.Image1 is not null)
                    { fileIdx++; }

                    tempFileName += Path.GetExtension(photo.FileName);

                    string tempLocalFilePath = Path.Combine(FilePaths.ImagesPath, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();

                    using FileStream localFileStream = File.OpenWrite(tempLocalFilePath);

                    await sourceStream.CopyToAsync(localFileStream);

                    ImagePaths ??= new();

                    if (fileIdx == 1)
                    {
                        ImagePaths.Image1 = new(tempLocalFilePath, photo.FileName);
                    }
                    else
                    {
                        ImagePaths.Image2 = new(tempLocalFilePath, photo.FileName);
                    }
                }

                _ = BuildImagePathsObsColAsync();
            }
        }
    }
}
