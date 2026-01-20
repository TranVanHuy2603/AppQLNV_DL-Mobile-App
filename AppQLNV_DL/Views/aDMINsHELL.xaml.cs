using AppQLNV_DL.Views;
using Microsoft.Maui.Controls;

namespace AppQLNV_DL {
    public partial class AdminShell : Shell
    {
        public AdminShell()
        {
            InitializeComponent();

            // Đăng ký route cho trang chi tiết nhân viên
            Routing.RegisterRoute(nameof(Views.EmployeeDetailPage), typeof(Views.EmployeeDetailPage));

            // Đăng ký route cho trang thêm nhân viên
            Routing.RegisterRoute(nameof(Views.AddEmployeePage), typeof(Views.AddEmployeePage));

            // Đăng ký route cho trang sửa nhân viên
            Routing.RegisterRoute(nameof(Views.EmployeeEditPage), typeof(Views.EmployeeEditPage));

            // Đăng ký route cho trang danh sách nhân viên (nếu cần điều hướng tới nó)
            Routing.RegisterRoute(nameof(Views.EmployeeListPage), typeof(Views.EmployeeListPage));

            Routing.RegisterRoute(nameof(Views.HomePage), typeof(Views.HomePage));

            Routing.RegisterRoute(nameof(Views.SettingsPage), typeof(Views.SettingsPage));

            Routing.RegisterRoute(nameof(Views.JobPage), typeof(Views.JobPage));

            Routing.RegisterRoute(nameof(Views.AddJobPage), typeof(Views.AddJobPage));

            Routing.RegisterRoute(nameof(Views.AdminChangePasswordPage), typeof(Views.AdminChangePasswordPage));

            Routing.RegisterRoute(nameof(Views.MonthlyCompletedPage), typeof(Views.MonthlyCompletedPage));

            Routing.RegisterRoute(nameof(Views.CompanyServicesPage), typeof(Views.CompanyServicesPage));

            Routing.RegisterRoute(nameof(Views.DevicePage), typeof(Views.DevicePage));

            Routing.RegisterRoute(nameof(Views.LastMonthJobPage), typeof(Views.LastMonthJobPage));
        }

    }

}
