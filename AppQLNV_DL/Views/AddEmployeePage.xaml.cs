using AppQLNV_DL.ViewModels;
using Microsoft.Maui.Controls;

namespace AppQLNV_DL.Views
{
    public partial class AddEmployeePage : ContentPage
    {
        public AddEmployeePage()
        {
            InitializeComponent();
            BindingContext = new AddEmployeeViewModel(); 
        }
    }

}
