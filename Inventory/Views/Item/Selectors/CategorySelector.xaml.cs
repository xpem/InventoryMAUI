using Inventory.ViewModels.Item.Selectors;

namespace Inventory.Views.Item.Selectors;

public partial class CategorySelector : ContentPage
{
    public CategorySelector(CategorySelectorVM categorySelectorVM)
    {
        InitializeComponent();

        BindingContext = categorySelectorVM;
    }
}