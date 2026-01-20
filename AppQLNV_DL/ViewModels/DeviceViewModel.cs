using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace AppQLNV_DL.ViewModels
{
    public partial class DeviceViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<AppQLNV_DL.Models.Device> devices = new ObservableCollection<AppQLNV_DL.Models.Device>();

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isLoading; 

        [ObservableProperty]
        private string newDeviceName = string.Empty;

        public DeviceViewModel()
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

        [RelayCommand]
        private async Task DeleteDevice(AppQLNV_DL.Models.Device deviceToDelete)
        {
            if (deviceToDelete == null)
            {
                return;
            }

            bool confirm = await Shell.Current.DisplayAlert("Xác nhận xóa", $"Bạn có chắc chắn muốn xóa dịch vụ '{deviceToDelete.DeviceName}' không?", "Có", "Không");

            if (confirm)
            {
                try
                {
                    IsLoading = true;
                    bool success = await AppQLNV_DL.Models.Device.DeleteAsync(deviceToDelete.DeviceId);

                    if (success)
                    {
                        Devices.Remove(deviceToDelete);
                        await Shell.Current.DisplayAlert("Thành công", "Dịch vụ đã được xóa.", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Lỗi", "Không thể xóa dịch vụ.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Lỗi xóa dịch vụ: {ex.Message}");
                    await Shell.Current.DisplayAlert("Lỗi", $"Đã xảy ra lỗi khi xóa dịch vụ: {ex.Message}", "OK");
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        [RelayCommand]
        private async Task AddDevice()
        {
            if (string.IsNullOrWhiteSpace(NewDeviceName))
            {
                await Shell.Current.DisplayAlert("Lỗi", "Vui lòng nhập tên thiết bị.", "OK");
                return;
            }

            try
            {
                IsLoading = true;

                var newDevice = new AppQLNV_DL.Models.Device
                {
                    DeviceName = NewDeviceName
                };

                var createdDevice = await AppQLNV_DL.Models.Device.CreateAsync(newDevice); 
                if (createdDevice != null)
                {
                    Devices.Add(createdDevice); 
                    NewDeviceName = string.Empty;
                    await Shell.Current.DisplayAlert("Thành công", "Thiết bị đã được thêm.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Lỗi", "Không thể thêm thiết bị.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Lỗi", $"Đã xảy ra lỗi: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }


        private async Task LoadDataAsync()
        {
            try
            {
                if (!IsRefreshing) IsLoading = true; 

                var fetchedDevices = await AppQLNV_DL.Models.Device.GetAllAsync();

                Devices.Clear();
                foreach (var device in fetchedDevices)
                {
                    Devices.Add(device);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Lỗi", "Không thể tải danh sách dịch vụ.", "OK");
            }
            finally
            {
                IsLoading = false; 
            }
        }
    }
}