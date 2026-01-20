using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace AppQLNV_DL.Views;

public partial class AdminChangePasswordPage : ContentPage
{
    public AdminChangePasswordPage()
    {
        InitializeComponent();
    }

    private async void OnSavePasswordClicked(object sender, EventArgs e)
    {
        // 1. Lấy dữ liệu từ giao diện
        string OldPassword = txtOldPass.Text?.Trim();
        string NewPassword = txtNewPass.Text?.Trim();
        string confirmPass = txtConfirmPass.Text?.Trim();

        // 2. Kiểm tra tính hợp lệ
        if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(confirmPass))
        {
            await DisplayAlert("Thông báo", "Vui lòng nhập đầy đủ các trường mật khẩu.", "OK");
            return;
        }

        if (NewPassword.Length < 4) // Bạn có thể chỉnh lại độ dài theo ý muốn
        {
            await DisplayAlert("Lỗi", "Mật khẩu mới quá ngắn.", "OK");
            return;
        }

        if (NewPassword != confirmPass)
        {
            await DisplayAlert("Lỗi", "Xác nhận mật khẩu mới không khớp.", "OK");
            return;
        }

        // 3. Thực hiện gọi API
        loading.IsRunning = true;


        bool success = await CallApiChangePassword(OldPassword, NewPassword);

        loading.IsRunning = false;

        if (success)
        {
            await DisplayAlert("Thành công", "Mật khẩu đã được thay đổi. Hãy nhớ mật khẩu mới của bạn.", "OK");

            txtOldPass.Text = txtNewPass.Text = txtConfirmPass.Text = string.Empty;

            await Navigation.PopAsync();
        }
    }

    private async Task<bool> CallApiChangePassword(string oldP, string newP)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Thay đổi link theo link ngrok hiện tại của bạn
                string apiUrl = $"https://arlinda-rimy-andria.ngrok-free.dev/api/AdminAccounts/change-password";

                var data = new { OldPassword = oldP, NewPassword = newP };
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Đọc lỗi từ API (ví dụ: mật khẩu cũ sai)
                    string message = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Thất bại", message, "Thử lại");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Không thể kết nối đến máy chủ: " + ex.Message, "OK");
            return false;
        }
    }
}