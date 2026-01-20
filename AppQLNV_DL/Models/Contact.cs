using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AppQLNV_DL.Models
{
    public class Contact
    {
        public string PhoneNumber { get; set; } = null!;
        public string ZaloLink { get; set; } = null!;

        public static async Task<Contact> GetAdminContactAsync()
        {
            try
            {
                using HttpClient client = new HttpClient();
                string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Contacts/get-single"; // Ví dụ về URL mới

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Contact>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi tải thông tin liên hệ Admin. Mã trạng thái: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Nội dung lỗi: {errorContent}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi HTTP khi tải thông tin liên hệ Admin: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi Deserialize JSON khi tải thông tin liên hệ Admin: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception tổng quát khi tải thông tin liên hệ Admin: {ex.Message}");
            }
            return null;
        }
    }
}
