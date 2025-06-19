using Inventory.ViewModels;

namespace Inventory.Views;

public partial class UpdatePassword : ContentPage
{
    public UpdatePassword(UpdatePasswordVM updatePasswordVM)
    {
        InitializeComponent();

        BindingContext = updatePasswordVM;
    }
}