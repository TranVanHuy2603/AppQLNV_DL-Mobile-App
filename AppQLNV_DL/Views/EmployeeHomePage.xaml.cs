using AppQLNV_DL.ViewModels;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using System.Text.Json;

namespace AppQLNV_DL.Views
{
    public partial class EmployeeHomePage : ContentPage
    {
        public EmployeeHomePage()
        {
            InitializeComponent();
            BindingContext = new EmployeeHomePageViewModel();
        }

    }
}
