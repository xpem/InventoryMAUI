using CommunityToolkit.Mvvm.Input;
using Inventory.Views;
using Services.Interfaces;

namespace Inventory.ViewModels
{
    public partial class SignInVM(IUserService userService) : VMBase
    {
        string email = "", password = "", btnSignInText = "Acessar";

        bool btnSignInEnabled = true;

        public string Email { get => email; set { if (email != value) { email = value; SetProperty(ref email, value); } } }

        public string Password { get => password; set { if (password != value) { password = value; SetProperty(ref password, value); } } }

        public string BtnSignInText { get => btnSignInText; set { if (btnSignInText != value) { btnSignInText = value; SetProperty(ref btnSignInText, value); } } }

        public bool BtnSignInEnabled { get => btnSignInEnabled; set { if (btnSignInEnabled != value) { btnSignInEnabled = value; SetProperty(ref btnSignInEnabled, value); } } }

        [RelayCommand]
        public async Task SignUp() => await Shell.Current.GoToAsync($"{nameof(SignUp)}");

        [RelayCommand]
        public async Task UpdatePassword() => await Shell.Current.GoToAsync($"{nameof(UpdatePassword)}");

        [RelayCommand]
        public async Task SignIn()
        {
            IsBusy = true;

            try
            {
                if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        if (Password.Length > 3)
                        {
                            btnSignInText = "Acessando...";
                            BtnSignInEnabled = false;

                            Models.Resps.ServResp resp = await userService.SignIn(Email, Password);

                            if (resp.Success)
                            {
                                if (resp.Content is not null and int)
                                    ((App)App.Current).Uid = (int)resp.Content;

                                await Shell.Current.GoToAsync($"{nameof(FirstSync)}", false);

                            }
                            else
                            {
                                string errorMessage = resp.Error == Models.Resps.ErrorTypes.WrongEmailOrPassword
                                    ? "Email/senha incorretos"
                                    : resp.Error == Models.Resps.ErrorTypes.ServerUnavaliable
                                    ? "Servidor indisponível, favor entrar em contato com o desenvolvedor."
                                    : "Erro não mapeado, favor entrar em contato com o desenvolvedor.";
                                await Application.Current.Windows[0].Page.DisplayAlert("Aviso", errorMessage, null, "Ok");
                            }

                            BtnSignInEnabled = true;
                            btnSignInText = "Acessar";
                            IsBusy = false;
                        }
                        else
                            await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Digite uma senha com mais de 3 dígitos", null, "Continuar");
                    }
                    else
                        await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "É necessário ter acesso a internet para efetuar o acesso.", null, "Ok");
                }
                else
                    await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Insira seu email e senha.", null, "Continuar");
            }
            catch (Exception ex)
            {
                throw;
            }

            IsBusy = false;
        }
    }
}
