using AppQLNV_DL.ViewModels;
using AppQLNV_DL.Views;
using Plugin.LocalNotification;
using Syncfusion.Maui.Core.Hosting;

namespace AppQLNV_DL
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddTransient<EmployeeListPage>();
            builder.Services.AddTransient<EmployeeListViewModel>();
            builder.Services.AddTransient<EmployeeDetailPage>();
            builder.Services.AddTransient<EmployeeDetailViewModel>();
            builder.Services.AddTransient<EmployeeEditPage>();
            builder.Services.AddTransient<EmployeeEditViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<EmployeeHomePageViewModel>();
            builder.Services.AddTransient<EmployeeHomePage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<CustomerSettingsPage>();
            builder.Services.AddTransient<CustomerSettingsViewModel>();
            builder.Services.AddTransient<JobPage>();
            builder.Services.AddTransient<JobViewModel>();
            builder.Services.AddTransient<AddJobPage>();
            builder.Services.AddTransient<AddJobViewModel>();
            builder.Services.AddTransient<FullEmployeeListPage>();
            builder.Services.AddTransient<FullEmployeeListViewModel>();
            builder.Services.AddTransient<EmployeeJobPage>();
            builder.Services.AddTransient<EmployeeJobViewModel>();
            builder.Services.AddTransient<EmployeeSettingsPage>();
            builder.Services.AddTransient<EmployeeSettingsViewModel>();
            builder.Services.AddTransient<ChangePasswordPage>();
            builder.Services.AddTransient<StatisticalPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<StatisticalViewModel>();
            builder.Services.AddTransient<MonthlyCompletedPage>();
            builder.Services.AddTransient<MonthlyCompletedViewModel>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<CompanyServicesPage>();
            builder.Services.AddTransient<CompanyServicesViewModel>();
            builder.Services.AddTransient<DevicePage>();
            builder.Services.AddTransient<DeviceViewModel>();
            builder.Services.AddTransient<LastMonthJobPage>();
            builder.Services.AddTransient<LastMonthJobViewModel>();

            return builder.Build();
        }
    }
}