using AppQLNV_DL.Views;

namespace AppQLNV_DL
{
    public partial class EmployeeShell : Shell
    {
        public EmployeeShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
        }
    }
}