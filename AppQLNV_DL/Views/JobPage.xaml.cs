using AppQLNV_DL.ViewModels;

namespace AppQLNV_DL.Views
{
    public partial class JobPage : ContentPage
    {
        public JobPage()
        {
            InitializeComponent();
            // Gán BindingContext ở đây
            BindingContext = new JobViewModel();
        }
    }
}