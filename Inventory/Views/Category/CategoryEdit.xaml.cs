using Inventory.ViewModels.Category;

namespace Inventory.Views.Category;

public partial class CategoryEdit : ContentPage
{
	public CategoryEdit(CategoryEditVM categoryEditVM)
	{
		InitializeComponent();
		BindingContext = categoryEditVM;
    }
}