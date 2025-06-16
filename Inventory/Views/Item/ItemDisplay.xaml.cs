using Inventory.ViewModels.Item;

namespace Inventory.Views.Item;

public partial class ItemDisplay : ContentPage
{
	public ItemDisplay(ItemDisplayVM itemDisplayVM)
	{
		InitializeComponent();

		BindingContext = itemDisplayVM;
    }
}