using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppQLNV_DL.Views;
using AppQLNV_DL.Models;
using System.Diagnostics;

namespace AppQLNV_DL.ViewModels;

public partial class StatisticalViewModel : ObservableObject
{
    // Thuộc tính lưu trữ số lượng
    [ObservableProperty]
    private int totalJobsCount;

    [ObservableProperty]
    private int processingJobsCount;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private bool isBusy;

    public StatisticalViewModel()
    {
         _=LoadStatisticsAsync(); 
    }

    [RelayCommand]
    private async Task Refresh()
    {
        IsRefreshing = true;
        await LoadStatisticsAsync();
        IsRefreshing = false;
    }

    [RelayCommand]
    public async Task LoadStatisticsAsync()
    {
        try
        {
            if (!IsRefreshing) IsBusy = true;

            string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Jobs";
            var allJobs = await Job.GetJobsAsync(apiUrl);

            if (allJobs != null)
            {
                TotalJobsCount = allJobs.Count;

                ProcessingJobsCount = allJobs.Count(j => j.Status == 0);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Lỗi tải thống kê: {ex.Message}");
        }
        finally
        {
            IsBusy = false; 
        }
    }

    [RelayCommand]
    private async Task GoToMonthlyPage()
    {
        await Shell.Current.GoToAsync(nameof(MonthlyCompletedPage));
    }

    [RelayCommand]
    private async Task GoToLastMonthJobPage()
    {
        await Shell.Current.GoToAsync(nameof(LastMonthJobPage));
    }
}