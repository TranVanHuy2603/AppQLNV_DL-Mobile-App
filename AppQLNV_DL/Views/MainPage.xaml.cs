using Newtonsoft.Json;
using System.Collections.ObjectModel;
using AppQLNV_DL.Models;

namespace AppQLNV_DL
{
    public partial class MainPage : ContentPage
    {

        string apiUrl = "https://arlinda-rimy-andria.ngrok-free/api/Employees";

        // Danh sách này sẽ tự động cập nhật lên giao diện
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public MainPage()
        {
            InitializeComponent();

            // Kết nối danh sách với giao diện
            EmployeeList.ItemsSource = Employees;

            // Tự động tải dữ liệu luôn khi vừa vào trang
            LoadDataFromServer();
        }

        // Sự kiện khi bấm nút "Làm mới"
        private void OnLoadDataClicked(object sender, EventArgs e)
        {
            LoadDataFromServer();
        }

        // Hàm gọi API
        private async void LoadDataFromServer()
        {
            if (apiUrl.Contains("DAN_LINK"))
            {
                await DisplayAlertAsync("Lỗi", "Quên dán link Ngrok trong MainPage.xaml.cs rồi kìa!", "OK");
                return;
            }

            try
            {
                LoadingSpinner.IsRunning = true; // Hiện vòng quay

                HttpClient client = new HttpClient();

                // Gửi lệnh GET lên Server
                string json = await client.GetStringAsync(apiUrl);

                // Biến đổi JSON thành danh sách C#
                var data = JsonConvert.DeserializeObject<List<Employee>>(json);

                // Xóa cũ, thêm mới
                Employees.Clear();
                foreach (var item in data)
                {
                    Employees.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Lỗi", "Không tải được dữ liệu: " + ex.Message, "OK");
            }
            finally
            {
                LoadingSpinner.IsRunning = false; // Tắt vòng quay
            }
        }
    }
}