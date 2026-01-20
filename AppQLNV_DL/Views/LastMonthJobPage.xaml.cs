namespace AppQLNV_DL.Views;

using AppQLNV_DL.ViewModels;

public partial class LastMonthJobPage : ContentPage
{
    public LastMonthJobPage(LastMonthJobViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}