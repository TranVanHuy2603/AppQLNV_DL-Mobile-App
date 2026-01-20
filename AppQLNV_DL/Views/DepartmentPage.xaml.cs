using Microsoft.Maui.Controls;

namespace AppQLNV_DL.Views;

public partial class DepartmentPage : ContentPage
{
    public DepartmentPage()
    {
        InitializeComponent();
    }

    private async void OnAddEmployeeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddEmployeePage));
    }
}
