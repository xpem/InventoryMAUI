namespace Inventory
{
    public partial class App : Application
    {
        public int? Uid { get; set; }

        public readonly string Version = "@0.2.0";

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}