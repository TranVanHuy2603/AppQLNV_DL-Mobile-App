using AppQLNV_DL.ViewModels;
namespace AppQLNV_DL.Views;

[QueryProperty(nameof(DepartmentIdToLoad), "deptId")]
public partial class EmployeeListPage : ContentPage
{
    private int _departmentIdToLoad;
    private readonly EmployeeListViewModel _viewModel;

    public int DepartmentIdToLoad
    {
        get => _departmentIdToLoad;
        set
        {
            _departmentIdToLoad = value;
            if (_viewModel is not null)
            {
                _viewModel.DeptId = value;
            }
        }
    }

    public EmployeeListPage(EmployeeListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Ensure ViewModel receives the value if the query was applied before viewmodel was ready
        _viewModel.DeptId = _departmentIdToLoad;
    }
}