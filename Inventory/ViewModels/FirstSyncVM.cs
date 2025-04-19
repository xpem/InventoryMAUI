using Inventory.Infra.Services;
using Inventory.Views;
using Models.DTO;
using Services.Interfaces;

namespace Inventory.ViewModels
{
    public partial class FirstSyncVM : VMBase
    {

        private decimal progress;

        private readonly ISyncService SyncService;

        public decimal Progress { get => progress; set { if (progress != value) { progress = value; OnPropertyChanged(nameof(Progress)); } } }

        public IUserService UserService { get; }

        public IBuildDbService BuildDbService { get; }

        public ISubCategoryService SubCategoryService { get; }

        public FirstSyncVM(IUserService userService, ISubCategoryService subCategoryService, ISyncService syncService)
        {
            UserService = userService;
            SubCategoryService = subCategoryService;
            SyncService = syncService;
            _ = SynchronizingProcess();
        }

        private async Task SynchronizingProcess()
        {
            try
            {
                UserDTO user = await UserService.GetAsync();

                if (user != null)
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {

                        await SubCategoryService.ApiToLocalAsync(user.Id, user.LastUpdate);

                        Progress = 0.25M;

                        await SubCategoryService.LocalToApiAsync();

                        Progress = 0.5M;

                        //await BookHistoricSyncBLL.ApiToLocalSync(user.Id, user.LastUpdate);

                        Progress = 0.75M;

                        UserService.UpdateLastUpdate(user.Id);

                        Progress = 1;

                        _ = Task.Run(() => { Task.Delay(5000); SyncService.StartThread(); });

                        //_ = AppShellVM.AtualizaUserShowData();


                        _ = Shell.Current.GoToAsync($"//{nameof(Main)}");

                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
