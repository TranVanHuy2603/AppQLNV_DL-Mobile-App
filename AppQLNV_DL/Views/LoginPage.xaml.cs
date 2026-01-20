using AppQLNV_DL.Models;
using AppQLNV_DL.Views;
using Plugin.Firebase.CloudMessaging;
using System.Text;
using System.Text.Json;

namespace AppQLNV_DL;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnAdminLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlertAsync("Lỗi", "Vui lòng nhập đầy đủ tài khoản và mật khẩu", "OK");
            return;
        }

        LoadingOverlay.IsVisible = true;

        bool isOk = await CheckAdminLogin(txtUsername.Text, txtPassword.Text);

        LoadingOverlay.IsVisible = false;

        if (isOk)
        {
            Application.Current.MainPage = new AdminShell();
        }
        else
        {
            await DisplayAlertAsync("Lỗi", "Tài khoản hoặc mật khẩu Admin không đúng", "Thử lại");
        }
    }

    private async void OnEmployeeLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlertAsync("Lỗi", "Vui lòng nhập Số điện thoại và mật khẩu", "OK");
            return;
        }

        LoadingOverlay.IsVisible = true;

        var employee = await CheckEmployeeLogin(txtUsername.Text, txtPassword.Text);

        LoadingOverlay.IsVisible = true; // Lưu ý: Dòng này có vẻ như bạn muốn đặt lại false sau khi hoàn thành

        if (employee != null)
        {
            Preferences.Default.Set("is_logged_in", true);
            Preferences.Default.Set("user_role", "Employee");
            Preferences.Default.Set("user_id", employee.EmployeeId);
            Preferences.Default.Set("user_name", employee.FullName);

            LoadingOverlay.IsVisible = false;
            Application.Current.MainPage = new EmployeeShell();
        }
        else
        {
            LoadingOverlay.IsVisible = false;
            await DisplayAlertAsync("Lỗi", "Số điện thoại hoặc mật khẩu không chính xác", "Thử lại");
        }
    }

    private async void OnCustomerClicked(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlertAsync("Lỗi", "Vui lòng nhập Số điện thoại và mật khẩu", "OK");
            return;
        }

        LoadingOverlay.IsVisible = true;

        var customer = await CheckCustomerLogin(txtUsername.Text, txtPassword.Text);

        LoadingOverlay.IsVisible = true; // Lưu ý: Dòng này có vẻ như bạn muốn đặt lại false sau khi hoàn thành

        if (customer != null)
        {
            Preferences.Default.Set("is_logged_in", true);
            Preferences.Default.Set("user_role", "Customer");
            Preferences.Default.Set("user_id", customer.CustomerId);
            Preferences.Default.Set("user_name", customer.CustomerName);

            LoadingOverlay.IsVisible  = false;
            Application.Current.MainPage = new CustomerShell();
        }
        else
        {
            LoadingOverlay.IsVisible = false;
            await DisplayAlertAsync("Lỗi", "Số điện thoại hoặc mật khẩu không chính xác", "Thử lại");
        }
    }

    public async Task<bool> CheckAdminLogin(string username, string password)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/AdminAccounts/login";
                var loginData = new { username = username, password = password };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Preferences.Default.Set("is_logged_in", true);
                    Preferences.Default.Set("user_role", "Admin");
                    return true;
                }
                return false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Lỗi Admin Login: " + ex.Message);
            return false;
        }
    }

    public async Task<Employee> CheckEmployeeLogin(string sdt, string password)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Employees/login";

                var loginData = new { phoneNumber = sdt, password = password };

                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(loginData, jsonOptions);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Employee>(resultJson, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                return null;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Lỗi Employee Login: " + ex.Message);
            return null;
        }
    } // <-- Dấu đóng ngoặc nhọn của CheckEmployeeLogin đã được di chuyển lên đây

    public async Task<Customer> CheckCustomerLogin(string sdt, string password)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Customers/login";

                var loginData = new { CustomerPhone = sdt, Password = password };

                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(loginData, jsonOptions);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Customer>(resultJson, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                return null;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Lỗi Customer Login: " + ex.Message);
            return null;
        }
    }

    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        await DisplayAlertAsync(
            "Quên mật khẩu",
            "Vui lòng liên hệ admin để được cấp lại mật khẩu.",
            "OK");
    }
}