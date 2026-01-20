// AppQLNV_DL/Views/CustomerHomePage.xaml.cs
using AppQLNV_DL.ViewModels;

namespace AppQLNV_DL.Views;

    public partial class CustomerHomePage : ContentPage
    {
        public CustomerHomePage()
        {
            InitializeComponent();
            BindingContext = new CustomerHomePageViewModel();
        }

    }
