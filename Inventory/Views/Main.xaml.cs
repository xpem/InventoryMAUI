using Inventory.ViewModels;

namespace Inventory.Views;

public partial class Main : ContentPage
{
    private readonly MainVM vm;

    public Main(MainVM vm)
    {
        InitializeComponent();

        BindingContext = this.vm = vm;
    }

    private void BtnItemSituationSelected_Clicked(object sender, EventArgs e)
    {
        View? view = sender as View;
        vm.ItemSituationSelectdCommand.Execute(view.BindingContext);
    }
}