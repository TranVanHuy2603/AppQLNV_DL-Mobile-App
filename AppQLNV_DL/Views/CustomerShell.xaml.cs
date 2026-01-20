using AppQLNV_DL.Views;

namespace AppQLNV_DL
{
    public partial class CustomerShell : Shell
    {
        public CustomerShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.CustomerHomePage), typeof(Views.CustomerHomePage));
            Routing.RegisterRoute(nameof(Views.CustomerSettingsPage), typeof(Views.CustomerSettingsPage));
        }
    }
}