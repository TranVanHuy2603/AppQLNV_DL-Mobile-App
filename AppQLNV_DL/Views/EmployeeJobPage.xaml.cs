using AppQLNV_DL.ViewModels;

namespace AppQLNV_DL.Views
{
    public partial class EmployeeJobPage : ContentPage
    {
        public EmployeeJobPage()
        {
            InitializeComponent();
            BindingContext = new EmployeeJobViewModel();
        }
    }
}