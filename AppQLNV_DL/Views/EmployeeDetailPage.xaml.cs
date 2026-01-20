using AppQLNV_DL.ViewModels;

namespace AppQLNV_DL.Views;

public partial class EmployeeDetailPage : ContentPage
{
    public EmployeeDetailPage(EmployeeDetailViewModel viewModel)
    {
        InitializeComponent();

        // Gán ViewModel tại đây (chuẩn MAUI)
        BindingContext = viewModel;
        //BindingContext = new EmployeeDetailViewModel();
    }
}
