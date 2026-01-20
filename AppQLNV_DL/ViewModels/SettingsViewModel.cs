using AppQLNV_DL.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace AppQLNV_DL.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isDarkModeEnabled;

    public SettingsViewModel()
    {
    }

    [RelayCommand]
    private async Task Logout()
    {
        bool confirmed = await Shell.Current.DisplayAlertAsync(
            "Xác nhận Đăng xuất",
            "Bạn có chắc chắn muốn đăng xuất khỏi tài khoản này?",
            "Đồng ý", "Hủy");

        if (confirmed)
        {
            Preferences.Default.Set("is_logged_in", false);
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

    [RelayCommand]
    private async Task GoToAdminChangePassword()
    {
       
        await Shell.Current.GoToAsync(nameof(AdminChangePasswordPage));
    }
    [RelayCommand]
    private async Task GoToCompanyServices()
    {
        await Shell.Current.GoToAsync(nameof(CompanyServicesPage));
    }

    [RelayCommand]
    private async Task GoToCompanyEquipment()
    {
        await Shell.Current.GoToAsync(nameof(DevicePage));
    }
}