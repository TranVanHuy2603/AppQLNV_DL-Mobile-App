using System.Text;
using System.Text.Json;
using System.Diagnostics;

namespace AppQLNV_DL.Models
{
    public class Job
    {
        private static readonly HttpClient client = new HttpClient();

        public int JobId { get; set; }
        public string Location { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public int Status { get; set; }
        public decimal? TotalCost { get; set; }
        public string? Note { get; set; } = null!;
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public int DeviceId { get; set; }
        public int ServiceId { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? UseMaterialId { get; set; }

        public string EmployeeName { get; set; } = "Chuưa xác định";
        public string DeviceName { get; set; } = "Chuưa xác định";
        public string ServiceName { get; set; } = "Chuưa xác định";
        public string EmployeePhone { get; set; } = "Chuưa xác định";
        public bool IsMine { get; set; }

        public static async Task<bool> CreateAsync(
            string location,
            string customerPhone,
            string note,
            int deviceId,
            int serviceId,
            int employeeId,
            int status)
        {
            string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Jobs";

            try
            {
                var payload = new
                {
                    Location = location,
                    CustomerPhone = customerPhone,
                    DeviceId = deviceId,
                    ServiceId = serviceId,
                    EmployeeId = employeeId,
                    AssignedDate = DateTime.Now,
                    TotalCost = 0.00,
                    Note = note,
                    Status = status
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var json = JsonSerializer.Serialize(payload, options);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                string respText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> CreateAsync2(
            string location,
            string customerPhone,
            string note,
            int deviceId,
            int serviceId,
            int customerId,
            int status)
        {
            string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Jobs";

            try
            {
                var payload = new
                {
                    Location = location,
                    CustomerPhone = customerPhone,
                    DeviceId = deviceId,
                    ServiceId = serviceId,
                    CustomerId = customerId,
                    AssignedDate = DateTime.Now,
                    TotalCost = 0.00,
                    Note = note,
                    Status = status
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var json = JsonSerializer.Serialize(payload, options);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                string respText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<List<Job>> GetJobsAsync(string apiUrl)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            };

            return JsonSerializer.Deserialize<List<Job>>(json, options) ?? new List<Job>();
        }

        public static async Task<bool> PutAsync(Job job)
        {
            string apiUrl = $"https://arlinda-rimy-andria.ngrok-free.dev/api/Jobs/{job.JobId}";

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var json = JsonSerializer.Serialize(job, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode) return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}