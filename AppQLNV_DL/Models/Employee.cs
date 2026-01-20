using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using AppQLNV_DL.DTOs;

namespace AppQLNV_DL.Models
{
    public class Employee
    {
        private static readonly HttpClient client = new HttpClient();

        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CitizenId { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string Status { get; set; } = "Active";
        public int DepartmentId { get; set; }
        public string Password { get; set; } = null!;
        public string? FcmToken { get; set; }
        public static async Task<bool> AddEmployeeAsync(Employee emp) 
        {
            try
            {
                var empDto = new EmployeeCreateDto
                {
                    FullName = emp.FullName,
                    PhoneNumber = emp.PhoneNumber,
                    CitizenId = emp.CitizenId,
                    AvatarUrl = emp.AvatarUrl,
                    Status = emp.Status,
                    DepartmentId = emp.DepartmentId,
                    Password = emp.CitizenId 
                };

                string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Employees";

                var json = JsonSerializer.Serialize(empDto, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                string respText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<List<Employee>> GetEmployeesByDepartmentIdAsync(int deptId)
        {
            string apiUrl = $"https://arlinda-rimy-andria.ngrok-free.dev/api/Employees?departmentId={deptId}";
            try
            {
                var response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode) return new List<Employee>();

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Employee>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                       ?? new List<Employee>();
            }
            catch
            {
                return new List<Employee>();
            }
        }

        public static async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            string apiUrl = $"https://arlinda-rimy-andria.ngrok-free.dev/api/Employees/{id}";
            try
            {
                var response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode) return null;

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Employee>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> DeleteEmployeeAsync(int id)
        {
            string apiUrl = $"https://arlinda-rimy-andria.ngrok-free.dev/api/Employees/{id}";
            try
            {
                var response = await client.DeleteAsync(apiUrl);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> UpdateEmployeeAsync(int employeeId, Employee employeeToUpdate)
        {
            if (employeeId <= 0) return false;

            string apiUrl = $"https://arlinda-rimy-andria.ngrok-free.dev/api/Employees/{employeeId}";

            try
            {
                var empDto = new EmployeeEditDto
                {
                    FullName = employeeToUpdate.FullName,
                    PhoneNumber = employeeToUpdate.PhoneNumber,
                    CitizenId = employeeToUpdate.CitizenId,
                };

                var json = JsonSerializer.Serialize(empDto, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(apiUrl, content);

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

        public static async Task<bool> ResetPassWord(int employeeId)
        {
            if (employeeId <= 0) return false;

            string apiUrl =
                $"https://arlinda-rimy-andria.ngrok-free.dev/api/Employees/reset-password/{employeeId}";

            var response = await client.PostAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task<List<Employee>> GetAllAsync()
        {
            string apiUrl = "https://arlinda-rimy-andria.ngrok-free.dev/api/Employees";

            try
            {
                var response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode) return new List<Employee>();

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Employee>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new List<Employee>();
            }
            catch
            {
                return new List<Employee>();
            }
        }

    }

}