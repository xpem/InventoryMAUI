using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Infra.Services;
using Inventory.Views;
using Models.DTO;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels
{
    public partial class AppShellVM(ISyncService syncService, IBuildDbService buildDbBLL, IUserService userService) : ObservableObject
    {
        string email, name;

        public string Email { get => email; set { if (email != value) { SetProperty(ref (email), value); } } }

        public string Name { get => name; set { if (name != value) { SetProperty(ref (name), value); } } }


        public async Task AtualizaUserShowData()
        {
            UserDTO? user = await userService.GetAsync();

            if (user is not null)
            {
                Name = user.Name;
                Email = user.Email;
            }
        }

        [RelayCommand]
        private async Task SignOut()
        {
            bool resp = await Application.Current.Windows[0].Page.DisplayAlert("Confirmação", "Deseja sair e retornar a tela inicial?", "Sim", "Cancelar");

            if (resp)
            {
                //finalize sync thread process
                syncService.ThreadIsRunning = false;

                syncService.Timer?.Dispose();

                (App.Current as App).Uid = null;

                await buildDbBLL.CleanLocalDatabase();

                _ = Shell.Current.GoToAsync($"//{nameof(SignIn)}");
            }
        }
    }
}
