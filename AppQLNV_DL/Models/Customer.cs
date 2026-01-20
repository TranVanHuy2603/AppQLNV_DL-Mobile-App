using System.Text.Json;
using System.Text;

namespace AppQLNV_DL.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; }
        public string CustomerPassword { get; set; } = null!;

        public static async Task<bool> RegisterAsync(string name, string phone, string password)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Customers/register";

                    var registerData = new
                    {
                        CustomerName = name,
                        CustomerPhone = phone,
                        Password = password
                    };

                    var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    var json = JsonSerializer.Serialize(registerData, jsonOptions);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode) return true;
                    else return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
