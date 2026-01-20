using AppQLNV_DL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AppQLNV_DL.ViewModels
{
    public partial class DepartmentViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Department> departments = new ObservableCollection<Department>();

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        bool isBusy;
        public DepartmentViewModel()
        {
            _ = LoadDataAsync();
        }

        [RelayCommand]
        async Task Refresh()
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

                string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Departments";
                var list = await Department.GetDepartmentsAsync(apiUrl);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Departments.Clear();
                    foreach (var item in list)
                        Departments.Add(item);
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () => {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task DepartmentSelected(Department selectedDept)
        {
            if (selectedDept == null) return;
            await Shell.Current.GoToAsync($"{nameof(AppQLNV_DL.Views.EmployeeListPage)}?deptId={selectedDept.DepartmentId}");
        }
    }
}