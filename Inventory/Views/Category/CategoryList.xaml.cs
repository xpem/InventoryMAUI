using Inventory.ViewModels.Category;

namespace Inventory.Views.Category;

public partial class CategoryList : ContentPage
{
	public CategoryList(CategoryListVM categoryListVM)
	{
		InitializeComponent();
		BindingContext = categoryListVM;
    }
}