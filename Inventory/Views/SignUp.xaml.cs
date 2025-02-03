using Inventory.ViewModels;

namespace Inventory.Views;

public partial class SignUp : ContentPage
{
    public SignUp(SignUpVM signUpVM)
    {
        InitializeComponent();

        BindingContext = signUpVM;
    }
}