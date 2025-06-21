using Inventory.ViewModels.Category;

namespace Inventory.Views.Category;

public partial class CategoryDisplay : ContentPage
{
	public CategoryDisplay(CategoryDisplayVM categoryDisplayVM)
	{
		InitializeComponent();
		BindingContext = categoryDisplayVM;
    }
}