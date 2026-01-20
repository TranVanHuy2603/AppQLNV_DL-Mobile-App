using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceModel = AppQLNV_DL.Models.Device;
using EmployeeModel = AppQLNV_DL.Models.Employee;
using ServiceModel = AppQLNV_DL.Models.Service;

namespace AppQLNV_DL.ViewModels;

public partial class AddJobViewModel : ObservableObject
{
    [ObservableProperty]
    string location;

    [ObservableProperty]
    string customerPhone;

    [ObservableProperty]
    string note;

    [ObservableProperty]
    DeviceModel selectedDevice;

    [ObservableProperty]
    ServiceModel selectedService;

    [ObservableProperty]
    EmployeeModel selectedEmployee;

    [ObservableProperty]
    List<DeviceModel> devices = new();

    [ObservableProperty]
    List<ServiceModel> services = new();

    [ObservableProperty]
    List<EmployeeModel> employees = new();

    public AddJobViewModel()
    {
        _ = LoadDataAsync();
    }

    async Task LoadDataAsync()
    {
        Devices = await DeviceModel.GetAllAsync();
        Services = await ServiceModel.GetAllAsync();
        Employees = await EmployeeModel.GetAllAsync();
    }

    [RelayCommand]
    async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Location)
            || SelectedDevice == null
            || SelectedService == null
            || SelectedEmployee == null)
        {
            await Shell.Current.DisplayAlertAsync(
                "Lỗi",
                "Vui lòng chọn đầy đủ thông tin",
                "OK");
            return;
        }

        bool success = await Job.CreateAsync(
            Location,
            CustomerPhone,
            Note,
            SelectedDevice.DeviceId,
            SelectedService.ServiceId,
            SelectedEmployee.EmployeeId,
            0
        );

        if (success)
        {
            await Shell.Current.DisplayAlertAsync(
                "Thành công",
                "Đã giao việc thành công",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlertAsync(
                "Lỗi",
                "Không giao được công việc",
                "OK");
        }
    }
}