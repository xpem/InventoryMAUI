using Inventory.ViewModels.Item;

namespace Inventory.Views.Item;

public partial class ItemEdit : ContentPage
{
	public ItemEdit(ItemEditVM itemEditVM)
	{
		InitializeComponent();
        BindingContext = itemEditVM;
    }

    private void BtnDelItemImage_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string ID = button.CommandParameter.ToString();

        var vm = (ItemEditVM)BindingContext;
        vm.DelItemImageCommand.Execute(ID);
    }
}