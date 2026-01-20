using AppQLNV_DL.ViewModels;

namespace AppQLNV_DL.Views
{
    public partial class EmployeeSettingsPage : ContentPage
    {
        public EmployeeSettingsPage(EmployeeSettingsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
