using CommunityToolkit.Mvvm.Input;
using Inventory.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.ViewModels
{
    public partial class SignInVM : VMBase
    {
        string email = "", password = "", btnSignInText = "Acessar";

        bool btnSignInEnabled = true;

        public string Email { get => email; set { if (email != value) { email = value; SetProperty(ref (email), value); } } }

        public string Password { get => password; set { if (password != value) { password = value; SetProperty(ref (password), value); } } }

        public string BtnSignInText { get => btnSignInText; set { if (btnSignInText != value) { btnSignInText = value; SetProperty(ref (btnSignInText), value); } } }

        public bool BtnSignInEnabled { get => btnSignInEnabled; set { if (btnSignInEnabled != value) { btnSignInEnabled = value; SetProperty(ref (btnSignInEnabled), value); } } }

        [RelayCommand]
        public async Task SignUp() => await Shell.Current.GoToAsync($"{nameof(SignUp)}");

        [RelayCommand]
        public async Task UpdatePassword() { }// => await Shell.Current.GoToAsync($"{nameof(UpdatePassword)}");

        [RelayCommand]
        public async Task SignIn()
        {
            IsBusy = true;

            //try
            //{
            //    if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            //    {
            //        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            //        {
            //            if (Password.Length > 3)
            //            {
            //                btnSignInText = "Acessando...";
            //                BtnSignInEnabled = false;

            //                var resp = await userBLL.SignIn(Email, Password);

            //                if (resp.Success)
            //                {
            //                    if (resp.Content is not null and int)
            //                        ((App)App.Current).Uid = (int)resp.Content;

            //                    await Shell.Current.GoToAsync($"{nameof(FirstSync)}", false);

            //                    //await Shell.Current.GoToAsync($"//{nameof(Main)}");

            //                }
            //                else
            //                {
            //                    string errorMessage = "";

            //                    if (resp.Error == Models.Responses.ErrorTypes.WrongEmailOrPassword)
            //                        errorMessage = "Email/senha incorretos";
            //                    else if (resp.Error == Models.Responses.ErrorTypes.ServerUnavaliable)
            //                        errorMessage = "Servidor indisponível, favor entrar em contato com o desenvolvedor.";
            //                    else errorMessage = "Erro não mapeado, favor entrar em contato com o desenvolvedor.";

            //                    await Application.Current.Windows[0].Page.DisplayAlert("Aviso", errorMessage, null, "Ok");
            //                }

            //                BtnSignInEnabled = true;
            //                btnSignInText = "Acessar";
            //                IsBusy = false;
            //            }
            //            else
            //                await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Digite uma senha com mais de 3 dígitos", null, "Continuar");
            //        }
            //        else
            //            await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "É necessário ter acesso a internet para efetuar o acesso.", null, "Ok");
            //    }
            //    else
            //        await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Insira seu email e senha.", null, "Continuar");
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

            IsBusy = false;
        }
    }
}
