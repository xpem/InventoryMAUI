using Inventory.ViewModels;

namespace Inventory
{
    public partial class AppShell : Shell
    {

        public AppShell(AppShellVM appShellVM)
        {
            InitializeComponent();

            BindingContext = appShellVM;
        }
    }
}
