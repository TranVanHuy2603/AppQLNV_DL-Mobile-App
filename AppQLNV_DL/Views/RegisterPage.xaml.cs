using System.Xml.Linq;
using AppQLNV_DL.Models;

namespace AppQLNV_DL;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string name = txtName.Text?.Trim();
        string phone = txtPhone.Text?.Trim();
        string password = txtPassword.Text;
        string confirm = txtConfirmPassword.Text;

        // ===== VALIDATE =====
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(phone) ||
            string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ thông tin", "OK");
            return;
        }

        if (password != confirm)
        {
            await DisplayAlert("Lỗi", "Mật khẩu không khớp", "OK");
            return;
        }

        loading.IsVisible = true;
        loading.IsRunning = true;

        try
        {
        bool success = await Customer.RegisterAsync(name, phone, password);

            if (success)
            {
                await DisplayAlert("Thành công", "Đăng ký thành công!", "OK");
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                await DisplayAlert("Thất bại", "Số điện thoại đã được sử dụng!", "OK");
            }
        }
        catch (Exception ex)
            {
            await DisplayAlert("Lỗi", ex.Message, "OK");
            }
            finally
            {
                loading.IsRunning = false;
                loading.IsVisible = false;
            }
    }

    private async void OnLoginTapped(object sender, TappedEventArgs e)
    {
        // Điều hướng trở lại trang đăng nhập
        await Navigation.PushAsync(new LoginPage());
    }
}
