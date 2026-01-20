using AppQLNV_DL.Models; // Đảm bảo bạn đã có các Model: Job, Employee
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace AppQLNV_DL.ViewModels;

public partial class HomePageViewModel : ObservableObject
{
    // === THUỘC TÍNH CŨ ===
    [ObservableProperty] private string greetingText;
    [ObservableProperty] private string weatherText;
    [ObservableProperty] private string staffName;
    public DateTime CurrentDate { get; } = DateTime.Now;

    // === THUỘC TÍNH MỚI ===
    [ObservableProperty] private bool isRefreshing;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private ObservableCollection<Job> jobs = new();

    public HomePageViewModel()
    {
        SetGreetingBasedOnRealTime();
        LoadStaffName();

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

            await Task.WhenAll(listTask, employeesTask, devicesTask, servicesTask);

            var list = await listTask;
            var allEmployees = await employeesTask;
            var allDevices = await devicesTask;
            var allServices = await servicesTask;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Jobs.Clear();
                foreach (var job in list.Where(j => j.Status == -1))
                {
                    job.EmployeeName = allEmployees.FirstOrDefault(e => e.EmployeeId == job.EmployeeId)?.FullName ?? "Chưa xác định";
                    job.DeviceName = allDevices.FirstOrDefault(d => d.DeviceId == job.DeviceId)?.DeviceName ?? "Chưa xác định";
                    job.ServiceName = allServices.FirstOrDefault(s => s.ServiceId == job.ServiceId)?.ServiceName ?? "Chưa xác định";
                    Jobs.Add(job);
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
    async Task Refresh()
    {
        await InitializeDataAsync();
    }

    [RelayCommand]
    async Task AssignJob(Job selectedJob)
    {
        if (selectedJob == null) return;

        var employees = await Employee.GetAllAsync();
        if (employees == null || employees.Count == 0) return;

        string[] employeeNames = employees.Select(e => e.FullName).ToArray();

        string choice = await Shell.Current.DisplayActionSheet(
            "Giao việc cho nhân viên",
            "Hủy",
            null,
            employeeNames);

        if (choice != "Hủy" && !string.IsNullOrEmpty(choice))
        {
            var selectedEmp = employees.FirstOrDefault(e => e.FullName == choice);
            if (selectedEmp != null)
            {
                selectedJob.EmployeeId = selectedEmp.EmployeeId;
                selectedJob.Status = 0;

                bool success = await Job.PutAsync(selectedJob);

                if (success)
                {
                    await Shell.Current.DisplayAlertAsync("Thành công", $"Đã giao việc cho {choice}", "OK");

                    await LoadDataAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Lỗi", "Không thể cập nhật trạng thái trên máy chủ", "OK");
                }
            }
        }
    }

    [RelayCommand]
    public async Task CallCustomer(Job job)
    {
        if (job == null || string.IsNullOrWhiteSpace(job.CustomerPhone))
        {
            await Shell.Current.DisplayAlert("Thông báo", "Công việc này không có số điện thoại", "OK");
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
                await Shell.Current.DisplayAlert("Lỗi", "Thiết bị này không hỗ trợ gọi điện", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Lỗi", "Không thể thực hiện cuộc gọi: " + ex.Message, "OK");
        }
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
}