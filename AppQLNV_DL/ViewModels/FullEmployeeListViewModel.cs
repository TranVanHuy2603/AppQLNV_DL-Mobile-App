using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel.Communication;

namespace AppQLNV_DL.ViewModels;

public partial class FullEmployeeListViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

    [ObservableProperty]
    bool isBusy;

    [ObservableProperty]
    bool isRefreshing;

    public FullEmployeeListViewModel()
    {
        Employees = new ObservableCollection<Employee>();
        _ = LoadAllEmployeesAsync();
    }

    [RelayCommand]
    public async Task Refresh()
    {
        IsRefreshing = true;
        await LoadAllEmployeesAsync();
        IsRefreshing = false;
    }

    public async Task LoadAllEmployeesAsync()
    {
        if (IsBusy && !IsRefreshing) return;

        if (!IsRefreshing) IsBusy = true;

        try
        {
            var list = await Employee.GetAllAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Employees.Clear();
                foreach (var emp in list)
                {
                    Employees.Add(emp);
                }
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Lỗi", "Không thể tải danh sách: " + ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task CallEmployee(Employee emp)
    {
        if (emp == null || string.IsNullOrWhiteSpace(emp.PhoneNumber))
        {
            await Shell.Current.DisplayAlertAsync("Thông báo", "Nhân viên này không có số điện thoại", "OK");
            return;
        }

        try
        {
            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open(emp.PhoneNumber);
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Lỗi", "Thiết bị không hỗ trợ gọi điện", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Lỗi", "Lỗi thực hiện cuộc gọi: " + ex.Message, "OK");
        }
    }
}