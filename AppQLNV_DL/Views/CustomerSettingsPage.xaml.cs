using AppQLNV_DL.ViewModels;

namespace AppQLNV_DL.Views
{
    public partial class CustomerSettingsPage : ContentPage
    {
        public CustomerSettingsPage(CustomerSettingsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
