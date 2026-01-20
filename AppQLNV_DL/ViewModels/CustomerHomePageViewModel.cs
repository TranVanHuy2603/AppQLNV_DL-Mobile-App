using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AppQLNV_DL.ViewModels;

public partial class CustomerHomePageViewModel : ObservableObject
{
    [ObservableProperty] private string greetingText = "Đang tải...";
    [ObservableProperty] private string weatherText = "Đang tải...";
    [ObservableProperty] private string customerName;

    [ObservableProperty] private string adminPhoneNumber = "Đang tải...";
    [ObservableProperty] private string adminZaloLink = "Đang tải...";

    [ObservableProperty] private bool isRefreshing;
    [ObservableProperty] private bool isLoadingContent;

    [ObservableProperty] private ObservableCollection<Job> jobs = new();
    [ObservableProperty] private ObservableCollection<string> companyImages = new();
    [ObservableProperty] private ObservableCollection<string> serviceImages = new();
    public DateTime CurrentDate { get; } = DateTime.Now;
    private int CustomerId => Preferences.Default.Get("user_id", 0);
    public CustomerHomePageViewModel()
    {
        CustomerName = Preferences.Default.Get("user_name", "Khách hàng");
        LoadStaticImages();
        _ = InitializeAsync();
    }
    private async Task InitializeAsync()
    {
        IsLoadingContent = true;

        try
        {
            await LoadWeatherAsync();
            await LoadAdminContactAsync();
            await LoadJobsAsync();
        }
        finally
        {
            SetGreeting();
            IsLoadingContent = false;
        }
    }

    private async Task LoadJobsAsync()
    {
        try
        {
            if (!IsRefreshing) IsLoadingContent = true;

            var jobsTask = Job.GetJobsAsync("https://arlinda-rimy-andria.ngrok-free.dev/api/Jobs");
            var employeesTask = Employee.GetAllAsync();
            var devicesTask = AppQLNV_DL.Models.Device.GetAllAsync();
            var servicesTask = Service.GetAllAsync();

            await Task.WhenAll(jobsTask, employeesTask, devicesTask, servicesTask);

            var jobList = jobsTask.Result
                .Where(j => j.CustomerId == CustomerId && (j.Status == 0 || j.Status == -1))
                .ToList();

            var employees = employeesTask.Result.ToDictionary(e => e.EmployeeId);
            var devices = devicesTask.Result.ToDictionary(d => d.DeviceId);
            var services = servicesTask.Result.ToDictionary(s => s.ServiceId);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Jobs.Clear();

                Debug.WriteLine($"CustomerId in Preferences = {CustomerId}");
                Debug.WriteLine($"Total jobs from API = {jobsTask.Result.Count}");

                foreach (var job in jobList)
                {
                    if (job.EmployeeId.HasValue && employees.TryGetValue(job.EmployeeId.Value, out var emp))
                    {
                        job.EmployeeName = emp.FullName;
                        job.EmployeePhone = emp.PhoneNumber;
                    }
                    else
                    {
                        job.EmployeeName = "N/A";
                    }

                    job.DeviceName = devices.TryGetValue(job.DeviceId, out var dev)
                        ? dev.DeviceName : "N/A";

                    job.ServiceName = services.TryGetValue(job.ServiceId, out var ser)
                        ? ser.ServiceName : "N/A";

                    Jobs.Add(job);
                }
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Lỗi", "Không thể tải công việc.", "OK");
        }
        finally
        {
            IsLoadingContent = false;
        }
    }
    private async Task LoadWeatherAsync()
    {
        try
        {
            string apiKey = "837a387e33b84514aa964529252012";
            string url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q=Hanoi&lang=vi";

            using HttpClient client = new() { Timeout = TimeSpan.FromSeconds(10) };
            var json = await client.GetStringAsync(url);
            var data = JObject.Parse(json);

            WeatherText = $"{data["current"]?["temp_c"]}°C - {data["current"]?["condition"]?["text"]}";
        }
        catch
        {
            WeatherText = "N/A";
        }
    }
    private async Task LoadAdminContactAsync()
    {
        try
        {
            var contact = await AppQLNV_DL.Models.Contact.GetAdminContactAsync();
            AdminPhoneNumber = contact?.PhoneNumber ?? "N/A";
            AdminZaloLink = contact?.ZaloLink ?? "N/A";
        }
        catch
        {
            AdminPhoneNumber = "Lỗi";
            AdminZaloLink = "Lỗi";
        }
    }
    private void LoadStaticImages()
    {
        CompanyImages = new()
        {
            "company1.jpg",
            "company2.jpg",
            "company3.jpg"
        };

        ServiceImages = new()
        {
            "service1.jpg",
            "service2.jpg",
            "service3.jpg"
        };
    }
    [RelayCommand]
    private async Task Refresh()
    {
        IsRefreshing = true;
        await LoadJobsAsync();
        await LoadWeatherAsync();
        IsRefreshing = false;
    }


    [RelayCommand]
    private async Task CallAdminAsync()
    {
        if (string.IsNullOrWhiteSpace(AdminPhoneNumber) || AdminPhoneNumber == "N/A")
        {
            await Shell.Current.DisplayAlert("Thông báo", "Không có số điện thoại.", "OK");
            return;
        }

        try
        {
            PhoneDialer.Open(AdminPhoneNumber);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Lỗi", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task CallEmployeeAsync(Job job)
    {
        if (job == null)
        {
            await Shell.Current.DisplayAlert("Lỗi", "Không tìm thấy công việc.", "OK");
            return;
        }

        if (job.Status == -1)
        {
            await Shell.Current.DisplayAlert(
                "Thông báo",
                "Công việc chưa được xác nhận.",
                "OK");
            return;
        }


        if (string.IsNullOrWhiteSpace(job.EmployeePhone) || job.EmployeePhone == "N/A")
        {
            await Shell.Current.DisplayAlert(
                "Thông báo",
                "Chưa có số điện thoại nhân viên.",
                "OK");
            return;
        }

        try
        {
            PhoneDialer.Open(job.EmployeePhone);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Lỗi", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task OpenZaloAsync()
    {
        if (!Uri.TryCreate(AdminZaloLink, UriKind.Absolute, out var uri))
        {
            await Shell.Current.DisplayAlert("Thông báo", "Link Zalo không hợp lệ.", "OK");
            return;
        }

        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }


    private void SetGreeting()
    {
        int hour = DateTime.Now.Hour;

        GreetingText = hour switch
        {
            >= 5 and < 12 => "Chào buổi sáng!",
            >= 12 and < 18 => "Chào buổi chiều!",
            _ => "Chào buổi tối!"
        };
    }
}
