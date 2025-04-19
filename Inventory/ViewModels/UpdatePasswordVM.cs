using CommunityToolkit.Mvvm.Input;
using Inventory.Utils;
using Services.Interfaces;

namespace Inventory.ViewModels
{
    public partial class UpdatePasswordVM(IUserService userService) : VMBase
    {
        string email;

        string btnSendEmailText = "Enviar Email";

        public string Email { get => email; set { if (email != value) { SetProperty(ref (email), value); } } }

        public string BtnSendEmailText { get => btnSendEmailText; set { if (btnSendEmailText != value) { SetProperty(ref (btnSendEmailText), value); } } }

        [RelayCommand]
        public async Task UpdatePassword()
        {
            BtnSendEmailText = "Processando...";
            IsBusy = true;

            if (!(Connectivity.NetworkAccess == NetworkAccess.Internet))
            {
                await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Sem conexão com a internet", null, "Ok");
                return;
            }

            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Digite um email válido", null, "Ok");
                return;
            }
            else if (!Validations.ValidateEmail(Email))
            {
                await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Digite um email válido", null, "Ok");
                return;
            }
            else
            {
                _ = userService.RecoverPasswordAsync(Email);

                await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Email de alteração de senha enviado!", null, "Ok");

                await Shell.Current.GoToAsync("..");
            }

            BtnSendEmailText = "Enviar Email";
            IsBusy = false;
        }
    }
}

