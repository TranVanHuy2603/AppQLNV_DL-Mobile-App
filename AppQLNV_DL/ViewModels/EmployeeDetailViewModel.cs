using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppQLNV_DL.Views;
using System.Net.Http;


namespace AppQLNV_DL.ViewModels;


[QueryProperty(nameof(EmployeeId), "employeeId")]
public partial class EmployeeDetailViewModel : ObservableObject
{
    [ObservableProperty]
    Employee currentEmployee;

    [ObservableProperty]
    int employeeId;

    partial void OnEmployeeIdChanged(int value)
    {
        _ = LoadEmployeeDetailsAsync();
    }

    async Task LoadEmployeeDetailsAsync()
    {
        CurrentEmployee = await Employee.GetEmployeeByIdAsync(EmployeeId);
    }

    [RelayCommand]
    async Task Delete()
    {
        if (CurrentEmployee == null) return;

        bool confirmed = await Shell.Current.DisplayAlertAsync("Xác nhận", $"Xóa nhân viên '{CurrentEmployee.FullName}'?", "Đồng ý", "Hủy");
        if (confirmed)
        {
            bool success = await Employee.DeleteEmployeeAsync(CurrentEmployee.EmployeeId);
            if (success)
            {
                await Shell.Current.DisplayAlertAsync("Thành công", "Đã xóa.", "OK");
                await Shell.Current.GoToAsync(".."); 
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Lỗi", "Xóa không thành công.", "OK");
            }
        }
    }

    [RelayCommand]
    private async Task Edit()
    {
        if (CurrentEmployee == null) return;

        await Shell.Current.GoToAsync($"{nameof(EmployeeEditPage)}?employeeId={CurrentEmployee.EmployeeId}");
    }

    [RelayCommand]
    async Task ResetPassword()
    {
        if (CurrentEmployee == null) return;

        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Xác nhận",
            $"Reset mật khẩu cho nhân viên '{CurrentEmployee.FullName}'?\nMật khẩu sẽ được đặt lại bằng CCCD.",
            "Đồng ý",
            "Hủy"
        );

        if (!confirm) return;

        bool success = await Employee.ResetPassWord(CurrentEmployee.EmployeeId);
        if (success)
        {
            await Shell.Current.DisplayAlertAsync("Thành công", "Đã reset mật khẩu cho nhân viên", "OK");
            await Shell.Current.GoToAsync(".."); 
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("Lỗi", "Reset mật khẩu không thành công.", "OK");
        }
    }

}