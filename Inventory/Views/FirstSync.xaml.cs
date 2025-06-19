using Inventory.ViewModels;

namespace Inventory.Views;

public partial class FirstSync : ContentPage
{
    public FirstSync(FirstSyncVM firstSyncVM)
    {
        InitializeComponent();

        BindingContext = firstSyncVM;
    }
}