using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppQLNV_DL.Models
{
    public partial class Department
    {
        private static readonly HttpClient client = new HttpClient();
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public static async Task<List<Department>> GetDepartmentsAsync(string apiUrl)
        {
            try
            {
                string json = await client.GetStringAsync(apiUrl);

                var departments = JsonConvert.DeserializeObject<List<Department>>(json);

                return departments ?? new List<Department>();
            }
            catch
            {
                return new List<Department>();
            }
        }
    }
}