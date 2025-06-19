using Inventory.ViewModels.Item.Selectors;

namespace Inventory.Views.Item.Selectors;

public partial class SubCategorySelector : ContentPage
{
    public SubCategorySelector(SubCategorySelectorVM subCategorySelectorVM)
    {
        InitializeComponent();

        BindingContext = subCategorySelectorVM;
    }
}