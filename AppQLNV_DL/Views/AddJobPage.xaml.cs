using AppQLNV_DL.ViewModels;

namespace AppQLNV_DL.Views
{
    public partial class AddJobPage : ContentPage
    {
        public AddJobPage()
        {
            InitializeComponent();
            BindingContext = new AddJobViewModel();
        }
    }
}
