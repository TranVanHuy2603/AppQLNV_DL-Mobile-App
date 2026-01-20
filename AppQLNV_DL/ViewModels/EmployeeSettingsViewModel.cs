using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppQLNV_DL.Views;

namespace AppQLNV_DL.ViewModels;

public partial class EmployeeSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isDarkModeEnabled;

    public EmployeeSettingsViewModel()
    {
    }

    [RelayCommand]
    private async Task GoToChangePassword()
    {
        await Shell.Current.GoToAsync(nameof(ChangePasswordPage));
    }

    [RelayCommand]
    private async Task GoToProfile()
    {
        await Shell.Current.DisplayAlertAsync("Thông báo", "Chức năng thông tin cá nhân đang phát triển", "OK");
    }

    [RelayCommand]
    private async Task Logout()
    {
        Preferences.Default.Set("is_logged_in", false);
        bool confirmed = await Shell.Current.DisplayAlertAsync(
            "Xác nhận Đăng xuất",
            "Bạn có chắc chắn muốn đăng xuất khỏi tài khoản này?",
            "Đồng ý", "Hủy");

        if (confirmed)
        {
            Preferences.Default.Clear();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}