using Inventory.ViewModels;

namespace Inventory.Views;

public partial class SignIn : ContentPage
{
    public SignIn(SignInVM signInVM)
    {
        InitializeComponent();

        BindingContext = signInVM;
    }
}