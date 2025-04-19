using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Infra.Models
{
    public partial class UIItemSituation : ObservableObject
    {
        int id;

        public int Id
        {
            get => id; set
            {
                if (id != value) { SetProperty(ref (id), value); }
            }
        }

        string name;

        public string Name
        {
            get => name; set
            {
                if (name != value) { SetProperty(ref (name), value);  }
            }
        }


        Color backgoundColor;

        public Color BackgoundColor
        {
            get => backgoundColor; set
            {
                if (backgoundColor != value)
                {
                    SetProperty(ref (backgoundColor), value);
                }
            }
        }
    }
}
