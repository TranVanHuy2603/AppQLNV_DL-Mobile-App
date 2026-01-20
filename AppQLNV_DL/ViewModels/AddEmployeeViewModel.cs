using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;


namespace AppQLNV_DL.ViewModels;

public partial class AddEmployeeViewModel : ObservableObject
{
    private string fullName = string.Empty;
    public string FullName
    {
        get => fullName;
        set => SetProperty(ref fullName, value);
    }

    private string citizenId = string.Empty;
    public string CitizenId
    {
        get => citizenId;
        set => SetProperty(ref citizenId, value);
    }

    private string phoneNumber = string.Empty;
    public string PhoneNumber
    {
        get => phoneNumber;
        set => SetProperty(ref phoneNumber, value);
    }

    private Department selectedDepartment;
    public Department SelectedDepartment
    {
        get => selectedDepartment;
        set => SetProperty(ref selectedDepartment, value);
    }

    private ObservableCollection<Department> departments = new ObservableCollection<Department>();
    public ObservableCollection<Department> Departments
    {
        get => departments;
        set => SetProperty(ref departments, value);
    }

    public AddEmployeeViewModel()
    {
        _ = LoadDepartmentsAsync();
    }

    private async Task LoadDepartmentsAsync()
    {
        try
        {
            string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Departments";
            var list = await Department.GetDepartmentsAsync(apiUrl);

            Departments.Clear();
            foreach (var item in list)
                Departments.Add(item);
        }
        catch (Exception ex)
        {
            return;
        }
    }

    [RelayCommand]
    public async Task SaveEmployee()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                await Application.Current.MainPage.DisplayAlertAsync("Lỗi", "Tên nhân viên không được để trống.", "OK");
                return;
            }

            if (SelectedDepartment == null)
            {
                await Application.Current.MainPage.DisplayAlertAsync("Lỗi", "Vui lòng chọn phòng ban.", "OK");
                return;
            }

            var newEmployee = new Employee
            {
                FullName = FullName,
                PhoneNumber = PhoneNumber,
                CitizenId = CitizenId,
                DepartmentId = SelectedDepartment.DepartmentId,
                Status = "Active",
                AvatarUrl = "avt.png",
                Password = CitizenId 
            };

            bool success = await Employee.AddEmployeeAsync(newEmployee);

            if (success)
            {
                await Application.Current.MainPage.DisplayAlertAsync("Thành công", "Đã thêm nhân viên.", "OK");
                await Shell.Current.GoToAsync(".."); 
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlertAsync("Lỗi", $"Có lỗi xảy ra: {ex.Message}", "OK");
        }
    }
}
