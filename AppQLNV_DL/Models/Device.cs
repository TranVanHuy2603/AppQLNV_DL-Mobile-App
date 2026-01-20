using AppQLNV_DL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;

namespace AppQLNV_DL.Models
{
    public class Device
    {
        private static readonly HttpClient client = new HttpClient();
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = null!;
        public static async Task<List<Device>> GetAllAsync()
        {
            string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Devices";

            try
            {
                var response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode) return new List<Device>();

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Device>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new List<Device>();
            }
            catch
            {
                return new List<Device>();
            }
        }
        public static async Task<bool> DeleteAsync(int deviceId)
        {
            try
            {
                string url = $"https://arlinda-rimy-andria.ngrok-free.dev/api/Devices/{deviceId}";

                HttpResponseMessage response = await client.DeleteAsync(url);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<Device?> CreateAsync(Device newDevice)
        {
            try
            {
                string url = "https://arlinda-rimy-andria.ngrok-free.dev/api/Devices";

                var response = await client.PostAsJsonAsync(url, newDevice);
                if (!response.IsSuccessStatusCode) return null;

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Device>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }
    }
}