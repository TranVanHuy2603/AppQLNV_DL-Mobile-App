using AppQLNV_DL.Models;
using Microsoft.EntityFrameworkCore;
namespace  QLNV_DL.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<AppQLNV_DL.Models.Device> Devices { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
    }
}
