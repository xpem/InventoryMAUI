using Inventory.Infra.Services;
using Inventory.ViewModels;
using Models.DTO;
using Services.Interfaces;

namespace Inventory
{
    public partial class App : Application
    {
        public int? Uid { get; set; }

        public readonly string Version = "@0.2.5";

        private ISyncService SyncServices { get; set; }

        private IUserService UserBLL { get; set; }

        private IBuildDbService BuildDbBLL { get; set; }

        public App(ISyncService syncServices, IUserService userBLL, IBuildDbService buildDbBLL)
        {
            SyncServices = syncServices;
            UserBLL = userBLL;
            BuildDbBLL = buildDbBLL;

            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            AppShellVM appShellVM = new AppShellVM(SyncServices, BuildDbBLL, UserBLL);

            BuildDbBLL.Init();

            _ = appShellVM.AtualizaUserShowData();

            UserDTO user = UserBLL.GetAsync().Result;

            if (user != null)
            {
                Uid = user.Id;
                SyncServices.StartThread();
            }

            return new Window(new AppShell(appShellVM));
        }
    }
}