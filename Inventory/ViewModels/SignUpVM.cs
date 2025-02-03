using CommunityToolkit.Mvvm.Input;
using Inventory.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.ViewModels
{
    public class SignUpVM(IUserService userServ) : VMBase
    {
        string name, email, password, confirmPassword;
        bool btnIsEnabled = true;

        public string Name { get => name; set { if (name != value) { SetProperty(ref (name), value); } } }

        public string Email { get => email; set { if (email != value) { SetProperty(ref (email), value); } } }

        public string Password { get => password; set { if (password != value) { SetProperty(ref (password), value); } } }

        public string ConfirmPassword { get => confirmPassword; set { if (confirmPassword != value) { SetProperty(ref (confirmPassword), value); } } }

        public bool BtnIsEnabled { get => btnIsEnabled; set { if (btnIsEnabled != value) { SetProperty(ref (btnIsEnabled), value);  } } }

        [RelayCommand]
        public async Task SignUp()
        {
            if (!(Connectivity.NetworkAccess == NetworkAccess.Internet))
            {
                _ = await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Sem conexão com a internet", null, "Ok");
                return;
            }

            if (VerifyFileds())
            {
                btnIsEnabled = false;

                //
                Models.Resps.ServResp resp = userServ.AddUser(name, email, password);

                if (!resp.Success)
                    await Application.Current.Windows[0].Page.DisplayAlert("Erro", "Não foi possível cadastrar o usuário!", null, "Ok");
                else
                {
                    bool res = await Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Usuário cadastrado!", null, "Ok");

                    if (!res)
                        await Shell.Current.GoToAsync("..");
                }
            }
        }

        private bool VerifyFileds()
        {
            //usar contracts??

            bool validInformation = true;

            if (string.IsNullOrEmpty(Name))
                validInformation = false;

            if (string.IsNullOrEmpty(Email))
                validInformation = false;
            else if (!Validations.ValidateEmail(Email))
            {
                _ = Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Digite um email válido", null, "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
                validInformation = false;
            else if (Password.Length < 4)
                validInformation = false;

            if (string.IsNullOrEmpty(ConfirmPassword))
                validInformation = false;
            else if (!ConfirmPassword.Equals(Password, StringComparison.CurrentCultureIgnoreCase))
                validInformation = false;

            if (!validInformation)
                _ = Application.Current.Windows[0].Page.DisplayAlert("Aviso", "Preencha os campos e confirme a senha corretamente", null, "Ok");

            return validInformation;
        }

    }
}
