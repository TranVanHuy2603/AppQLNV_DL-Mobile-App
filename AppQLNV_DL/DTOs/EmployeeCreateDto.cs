namespace AppQLNV_DL.DTOs
{
    public class EmployeeCreateDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string CitizenId { get; set; }
        public string? AvatarUrl { get; set; }
        public string Status { get; set; }
        public int DepartmentId { get; set; }
        public string Password { get; set; }
    }
}