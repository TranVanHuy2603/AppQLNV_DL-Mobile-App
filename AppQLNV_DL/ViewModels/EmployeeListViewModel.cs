using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel.Communication;

namespace AppQLNV_DL.ViewModels;

[QueryProperty(nameof(DeptId), "deptId")]
public partial class EmployeeListViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    bool isRefreshing;

    private int deptId;
    public int DeptId
    {
        get => deptId;
        set
        {
            if (deptId != value)
            {
                deptId = value;
                OnPropertyChanged();
                _ = LoadEmployeesByDepartmentIdAsync(deptId);
            }
        }
    }

    [RelayCommand]
    public async Task Refresh()
    {
        IsRefreshing = true;
        await LoadEmployeesByDepartmentIdAsync(DeptId);
        IsRefreshing = false;
    }

    public EmployeeListViewModel()
    {
        Employees = new ObservableCollection<Employee>();
    }

    public async Task LoadEmployeesByDepartmentIdAsync(int id)
    {
        if (id <= 0) return;

        if (!IsRefreshing) IsBusy = true;

        try
        {
            var list = await Employee.GetEmployeesByDepartmentIdAsync(id);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Employees.Clear();
                foreach (var emp in list)
                {
                    if (emp.DepartmentId == id)
                    {
                        Employees.Add(emp);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlertAsync("Lỗi", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task EmployeeSelected(Employee selectedEmployee)
    {
        if (selectedEmployee == null) return;
        await Shell.Current.GoToAsync($"{nameof(AppQLNV_DL.Views.EmployeeDetailPage)}?employeeId={selectedEmployee.EmployeeId}");
    }

    [RelayCommand]
    public async Task CallEmployee(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return;

        if (PhoneDialer.Default.IsSupported)
        {
            PhoneDialer.Default.Open(phoneNumber);
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("Lỗi", "Thiết bị không hỗ trợ gọi điện", "OK");
        }
    }
}