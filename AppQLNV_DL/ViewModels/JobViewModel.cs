using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.ApplicationModel.Communication;

namespace AppQLNV_DL.ViewModels
{
    public partial class JobViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Job> jobs = new ObservableCollection<Job>();

        [ObservableProperty]
        private Job selectedJob;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isBusy;

        public JobViewModel()
        {
            _ = LoadDataAsync();
        }

        [RelayCommand]
        private async Task Refresh()
        {
            IsRefreshing = true;
            await LoadDataAsync();
            IsRefreshing = false;
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
                    foreach (var job in list.Where(j => j.Status == 0))
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
                Debug.WriteLine($"Lỗi LoadData: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
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
                job.CompletedDate = DateTime.Now;

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
        public async Task AddJob()
        {
            await Shell.Current.GoToAsync("AddJobPage");
        }

        [RelayCommand]
        public async Task GoToLocation(Job job)
        {
            if (job == null || string.IsNullOrWhiteSpace(job.Location))
            {
                await Shell.Current.DisplayAlertAsync("Thông báo", "Công việc này không có địa chỉ", "OK");
                return;
            }

            try
            {
                string encodedAddress = Uri.EscapeDataString(job.Location);

                string url = $"https://www.google.com/maps/search/?api=1&query={encodedAddress}";

                await Launcher.Default.OpenAsync(url);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Lỗi", "Không thể mở bản đồ: " + ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task CallCustomer(Job job) 
        {
            if (job == null || string.IsNullOrWhiteSpace(job.CustomerPhone))
            {
                await Shell.Current.DisplayAlertAsync("Thông báo", "Không có số điện thoại khách hàng", "OK");
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
                    await Shell.Current.DisplayAlertAsync("Lỗi", "Thiết bị không hỗ trợ gọi điện", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Lỗi", "Đã gặp xự cố, vui longf lực hiện sau", "OK");
            }
        }
    }
}