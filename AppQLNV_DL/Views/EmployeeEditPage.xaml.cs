// File: Views/EmployeeEditPage.xaml.cs

using AppQLNV_DL.ViewModels; // Đảm bảo bạn đã using namespace của ViewModel

namespace AppQLNV_DL.Views;

public partial class EmployeeEditPage : ContentPage
{
    public EmployeeEditPage(EmployeeEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}