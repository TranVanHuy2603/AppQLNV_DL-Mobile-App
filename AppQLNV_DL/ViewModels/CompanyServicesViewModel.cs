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
    public partial class CompanyServicesViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Service> services = new ObservableCollection<Service>();

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string newServiceName = string.Empty;
        public CompanyServicesViewModel()
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
        private async Task DeleteService(Service serviceToDelete)
        {
            if (serviceToDelete == null) return;

            bool confirm = await Shell.Current.DisplayAlertAsync("Xác nhận xóa", $"Bạn có chắc chắn muốn xóa dịch vụ '{serviceToDelete.ServiceName}' không?", "Có", "Không");

            if (confirm)
            {
                try
                {
                    IsLoading = true;
                    bool success = await Service.DeleteAsync(serviceToDelete.ServiceId);

                    if (success)
                    {
                        Services.Remove(serviceToDelete);
                        await Shell.Current.DisplayAlert("Thành công", "Dịch vụ đã được xóa.", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Lỗi", "Không thể xóa dịch vụ.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Lỗi", $"Đã xảy ra lỗi khi xóa dịch vụ: {ex.Message}", "OK");
                }
                finally
                {
                    IsLoading = false; 
                }
            }
        }


        private async Task LoadDataAsync()
        {
            try
            {
                if (!IsRefreshing) IsLoading = true; 

                var fetchedServices = await Service.GetAllAsync();

                Services.Clear();
                foreach (var service in fetchedServices)
                {
                    Services.Add(service);
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


        [RelayCommand]
        private async Task AddService()
        {
            if (string.IsNullOrWhiteSpace(NewServiceName))
            {
                await Shell.Current.DisplayAlert("Lỗi", "Vui lòng nhập tên thiết bị.", "OK");
                return;
            }

            try
            {
                IsLoading = true;

                var newService = new Service
                {
                    ServiceName = NewServiceName
                };

                var createdService = await Service.CreateAsync(newService);
                if (createdService != null)
                {
                    Services.Add(createdService); 
                    NewServiceName = string.Empty;
                    await Shell.Current.DisplayAlert("Thành công", "Dịch vụ đã được thêm.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Lỗi", "Không thể thêm Dịch vụ.", "OK");
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
    }
}