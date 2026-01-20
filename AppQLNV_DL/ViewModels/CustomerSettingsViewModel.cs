using AppQLNV_DL.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace AppQLNV_DL.ViewModels;

public partial class CustomerSettingsViewModel : ObservableObject
{
    public CustomerSettingsViewModel()
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
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}