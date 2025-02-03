using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels
{
    public partial class VMBase : ObservableObject
    {
        bool isBusy;

        public bool IsNotBusy => !isBusy;

        public bool IsBusy
        {
            get => isBusy; set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    SetProperty(ref (isBusy), value);
                }
            }
        }

        protected static bool IsOn => Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}
