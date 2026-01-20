using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AppQLNV_DL.ViewModels;

public partial class LastMonthJobViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Job> jobs = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private int total;
    public LastMonthJobViewModel()
    {
        _ = LoadMonthlyJobsAsync();
    }

    [RelayCommand]
    public async Task LoadMonthlyJobsAsync()
    {
        try
        {
            IsBusy = true;

            int currentUserId = Preferences.Default.Get("user_id", 0);
            var now = DateTime.Now;

            string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Jobs";
            var listTask = Job.GetJobsAsync(apiUrl);
            var employeesTask = Employee.GetAllAsync();
            var devicesTask = AppQLNV_DL.Models.Device.GetAllAsync();
            var servicesTask = Service.GetAllAsync();

            await Task.WhenAll(listTask, employeesTask, devicesTask, servicesTask);

            var allJobs = await listTask;
            var allEmployees = await employeesTask;
            var allDevices = await devicesTask;
            var allServices = await servicesTask;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var filteredJobs = allJobs.Where(j =>
                    j.Status == 1 &&
                    j.CompletedDate.HasValue &&
                    j.CompletedDate.Value.Month == now.Month &&
                    j.CompletedDate.Value.Year == now.Year);

                var tempDetails = new List<Job>();

                foreach (var job in filteredJobs)
                {
                    job.EmployeeName = allEmployees.FirstOrDefault(e => e.EmployeeId == job.EmployeeId)?.FullName ?? "Không rõ";
                    job.DeviceName = allDevices.FirstOrDefault(d => d.DeviceId == job.DeviceId)?.DeviceName ?? "Thiết bị lạ";
                    job.ServiceName = allServices.FirstOrDefault(s => s.ServiceId == job.ServiceId)?.ServiceName ?? "Dịch vụ lạ";

                    tempDetails.Add(job);
                }
                Total = tempDetails.Count();

                Jobs = new ObservableCollection<Job>(tempDetails);
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Lỗi: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}