using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceModel = AppQLNV_DL.Models.Device;
using EmployeeModel = AppQLNV_DL.Models.Employee;
using ServiceModel = AppQLNV_DL.Models.Service;

namespace AppQLNV_DL.ViewModels;

public partial class CustomerAddJobViewModel : ObservableObject
{
    [ObservableProperty]
    string location;

    [ObservableProperty]
    string customerPhone;

    [ObservableProperty]
    string note;

    [ObservableProperty]
    int customerId;

    [ObservableProperty]
    DeviceModel selectedDevice;

    [ObservableProperty]
    ServiceModel selectedService;

    [ObservableProperty]
    List<DeviceModel> devices = new();

    [ObservableProperty]
    List<ServiceModel> services = new();

    public CustomerAddJobViewModel()
    {
        _ = LoadDataAsync();
    }

    async Task LoadDataAsync()
    {
        Devices = await DeviceModel.GetAllAsync();
        Services = await ServiceModel.GetAllAsync();
    }

    [RelayCommand]
    async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Location)
            || SelectedDevice == null
            || SelectedService == null)
        {
            await Shell.Current.DisplayAlertAsync(
                "Lỗi",
                "Vui lòng chọn đầy đủ thông tin",
                "OK");
            return;
        }

        CustomerId = Preferences.Default.Get("user_id", 0);

        bool success = await Job.CreateAsync2(
            Location,
            CustomerPhone,
            Note,
            SelectedDevice.DeviceId,
            SelectedService.ServiceId,
            CustomerId,
            -1
        );

        if (success)
        {
            await Shell.Current.DisplayAlertAsync(
                "Thành công",
                "Đã đặt việc thành công. Nhân viên chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.",
                "OK");

                Location = string.Empty;     
                CustomerPhone = string.Empty;  
                Note = string.Empty;           
                SelectedDevice = null;      
                SelectedService = null;
        }
        else
        {
            await Shell.Current.DisplayAlertAsync(
                "Lỗi",
                "Không đặt được việc",
                "OK");
        }
    }
}