using Inventory.ViewModels.Category.SubCategory;

namespace Inventory.Views.Category.SubCategory;

public partial class SubCategoryEdit : ContentPage
{
	public SubCategoryEdit(SubCategoryEditVM subCategoryEditVM)
	{
		InitializeComponent();
		BindingContext = subCategoryEditVM;
    }
}