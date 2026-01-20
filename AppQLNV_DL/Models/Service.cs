using System.Buffers.Text;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AppQLNV_DL.Models
{
    public class Service
    {
        private static readonly HttpClient client = new HttpClient();

        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public static async Task<List<Service>> GetAllAsync()
        {
            string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Services";

            try
            {
                var response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode) return new List<Service>();

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Service>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new List<Service>();
            }
            catch
            {
                return new List<Service>();
            }
        }

        public static async Task<bool> DeleteAsync(int serviceId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.DeleteAsync($"https://arlinda-rimy-andria.ngrok-free.dev/api/Services/{serviceId}");

                    if (response.IsSuccessStatusCode) return true;
                    else return false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<Service?> CreateAsync(Service newService)
        {
            try
            {
                var response = await client.PostAsJsonAsync("https://arlinda-rimy-andria.ngrok-free.dev/api/Services", newService);
                if (!response.IsSuccessStatusCode) return null;

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Service>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }
    }
}