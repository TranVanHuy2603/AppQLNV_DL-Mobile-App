using AppQLNV_DL.ViewModels;
namespace AppQLNV_DL.Views
{
    public partial class FullEmployeeListPage : ContentPage
    {
        public FullEmployeeListPage(FullEmployeeListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}