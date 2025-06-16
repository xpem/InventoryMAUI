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
        bool isBusy, isNotBusy;

        //public bool IsNotBusy
        //{
        //    get => isNotBusy; set
        //    {
        //        if (isNotBusy != value)
        //        {
        //            SetProperty(ref (isNotBusy), value);
        //        }
        //    }
        //}

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    SetProperty(ref (isBusy), value);
                    SetProperty(ref (isNotBusy), !value);
                }
            }
        }

        protected static bool IsOn => Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}
