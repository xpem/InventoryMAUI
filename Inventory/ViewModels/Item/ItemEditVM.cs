using CommunityToolkit.Mvvm.ComponentModel;
using Inventory.Infra.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Item
{
    public partial class ItemEditVM(IItemService itemService) : VMBase, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
        }

        int ItemId { get; set; }

        int CategoryId { get; set; }

        int? SubCategoryId { get; set; }

        const int resaleStatusId = 5;

        readonly int[] outSituations = [4, 5, 3, 7];

        public enum MediaPickerType { pick, capture }

        [ObservableProperty]
        string name, description, categoryName, acquisitionStore, commentary, acquisitionValue, resaleValue, btnInsertText, btnInsertIcon;
      
        [ObservableProperty]
        bool btnPickItemImageIsEnabled, btnInsertIsEnabled = true, btnDeleteIsVisible, stlResaleValueIsVisible, stlWithdrawalDateIsVisible, crvwIsVisible, vSLAddImageIsVisible = true;

        [ObservableProperty]
        DateTime acquisitionDate, withdrawalDate;
       
        [ObservableProperty]
        int pkrItemSituationSelectedIndex, pkrAcquisitionTypeSelectedIndex;


        FixedIndexesImagePaths ImagePaths { get; set; } = new();

        [ObservableProperty]
        ObservableCollection<UIImagePath> imagePathsObsCol;

        [ObservableProperty]
        UIItemSituation itemSituation;


    }
}
