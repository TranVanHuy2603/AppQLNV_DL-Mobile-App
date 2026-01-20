using AppQLNV_DL;
using AppQLNV_DL.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace AppQLNV_DL;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        bool isLoggedIn = Preferences.Default.Get("is_logged_in", false);
        string role = Preferences.Default.Get("user_role", "");

        if (isLoggedIn)
        {
            if (role == "Admin")
            {
                // Vào thẳng trang quản lý của Admin
                MainPage = new AdminShell();
            }
            else
            {
                // Vào thẳng trang công việc của Nhân viên
                MainPage = new EmployeeShell(); // Hoặc một Shell riêng cho Nhân viên
            }
        }
        else
        {
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
