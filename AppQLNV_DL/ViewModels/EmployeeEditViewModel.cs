using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace AppQLNV_DL.ViewModels;

[QueryProperty(nameof(EmployeeId), "employeeId")]
public partial class EmployeeEditViewModel : ObservableObject
{
    [ObservableProperty]
    private Employee currentEmployee;

    private int employeeId;
    public int EmployeeId
    {
        get => employeeId;
        set
        {
            SetProperty(ref employeeId, value);
            _ = LoadEmployeeAsync();
        }
    }
    private async Task LoadEmployeeAsync()
    {
        if (EmployeeId > 0)
        {
            CurrentEmployee = await Employee.GetEmployeeByIdAsync(EmployeeId);
        }
    }

    [RelayCommand]
    private async Task SaveChanges()
    {
        if (CurrentEmployee == null) return;

        bool success = await Employee.UpdateEmployeeAsync(CurrentEmployee.EmployeeId, CurrentEmployee);

        if (success)
        {
            await Shell.Current.DisplayAlertAsync("Thành công", "Đã cập nhật thông tin nhân viên.", "OK");
            await Shell.Current.GoToAsync(".."); 
        }
        else
        {

        }
    }
}