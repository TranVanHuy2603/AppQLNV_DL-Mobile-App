using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using Plugin.Firebase.CloudMessaging;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AppQLNV_DL.ViewModels;

public partial class EmployeeHomePageViewModel : ObservableObject
{
    [ObservableProperty] private string greetingText;
    [ObservableProperty] private string weatherText;
    [ObservableProperty] private string staffName;
    public DateTime CurrentDate { get; } = DateTime.Now;

    [ObservableProperty] private bool isRefreshing;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private ObservableCollection<Job> jobs = new();

    public EmployeeHomePageViewModel()
    {
        LoadStaffName();
        SetGreetingBasedOnRealTime();
        _ = InitializeDataAsync();
    }

    private async Task InitializeDataAsync()
    {
        await Task.WhenAll(
            LoadRealTimeWeatherDataAsync(),
            LoadDataAsync()
        );
    }
    private async Task LoadDataAsync()
    {
        try
        {
            if (!IsRefreshing) IsBusy = true;

            string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Jobs";
            var listTask = Job.GetJobsAsync(apiUrl);
            var employeesTask = Employee.GetAllAsync();
            var devicesTask = AppQLNV_DL.Models.Device.GetAllAsync();
            var servicesTask = Service.GetAllAsync();
            int currentUserId = Preferences.Default.Get("user_id", 0);

            await Task.WhenAll(listTask, employeesTask, devicesTask, servicesTask);

            var list = await listTask;
            var allEmployees = await employeesTask;
            var allDevices = await devicesTask;
            var allServices = await servicesTask;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Jobs.Clear();
                foreach (var job in list.Where(j => j.Status == 0))
                {
                    if (job.EmployeeId == currentUserId)
                    {
                        var emp = allEmployees.FirstOrDefault(e => e.EmployeeId == job.EmployeeId);
                        job.EmployeeName = emp != null ? emp.FullName : "N/A";
                        var device = allDevices.FirstOrDefault(d => d.DeviceId == job.DeviceId);
                        job.DeviceName = device != null ? device.DeviceName : "N/A";
                        var service = allServices.FirstOrDefault(s => s.ServiceId == job.ServiceId);
                        job.ServiceName = service != null ? service.ServiceName : "N/A";
                        Jobs.Add(job);
                    }
                }
            });
        }
        catch (Exception ex)
        {

        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    public async Task CompleteJob(Job job)
    {
        if (job == null) return;

        bool confirm = await Shell.Current.DisplayAlertAsync("Xác nhận", "Xác nhận hoàn thành công việc này?", "Đúng", "Hủy");
        if (!confirm) return;

        try
        {
            string apiUrl = $"https://arlinda-rimy-andria.ngrok-free.dev/api/Jobs/{job.JobId}";
            using HttpClient client = new HttpClient();
            job.Status = 1;

            var json = JsonSerializer.Serialize(job, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Jobs.Remove(job);
                await Shell.Current.DisplayAlertAsync("Thông báo", "Công việc đã hoàn thành.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Lỗi", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task Refresh()
    {
        await InitializeDataAsync();
    }

    [RelayCommand]
    public async Task CallCustomer(Job job)
    {
        if (job == null || string.IsNullOrWhiteSpace(job.CustomerPhone))
        {
            await Shell.Current.DisplayAlertAsync("Thông báo", "Công việc này không có số điện thoại", "OK");
            return;
        }

        try
        {
            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open(job.CustomerPhone);
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Lỗi", "Thiết bị này không hỗ trợ gọi điện", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Lỗi", "Không thể thực hiện cuộc gọi: " + ex.Message, "OK");
        }
    }

    private async Task LoadRealTimeWeatherDataAsync()
    {
        WeatherText = "...";
        string apiKey = "837a387e33b84514aa964529252012"; 
        string apiUrl = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q=Hanoi&lang=vi";

        try
        {
            using HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            string json = await client.GetStringAsync(apiUrl);
            var data = JObject.Parse(json);
            WeatherText = $"{data["current"]?["temp_c"]}°C - {data["current"]?["condition"]?["text"]}";
        }
        catch { WeatherText = "N/A"; }
    }

    private void LoadStaffName()
    {
        string role = Preferences.Default.Get("user_role", "");
        StaffName = (role == "Employee") ? Preferences.Default.Get("user_name", "") : "Quản trị viên";
    }

    private void SetGreetingBasedOnRealTime()
    {
        var hour = DateTime.Now.Hour;
        if (hour >= 5 && hour < 12) GreetingText = "Chào buổi sáng!";
        else if (hour >= 12 && hour < 18) GreetingText = "Chào buổi chiều!";
        else GreetingText = "Chào buổi tối!";
    }

}