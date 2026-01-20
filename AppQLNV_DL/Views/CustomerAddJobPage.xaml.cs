using AppQLNV_DL.ViewModels;

namespace AppQLNV_DL.Views
{
    public partial class CustomerAddJobPage : ContentPage
    {
        public CustomerAddJobPage()
        {
            InitializeComponent();
            BindingContext = new CustomerAddJobViewModel();
        }
    }
}
