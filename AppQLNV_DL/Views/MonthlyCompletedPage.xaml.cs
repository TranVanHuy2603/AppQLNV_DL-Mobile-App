namespace AppQLNV_DL.Views;

using AppQLNV_DL.ViewModels;

public partial class MonthlyCompletedPage : ContentPage
{
    public MonthlyCompletedPage(MonthlyCompletedViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}