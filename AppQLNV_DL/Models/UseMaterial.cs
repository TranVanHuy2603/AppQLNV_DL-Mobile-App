using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppQLNV_DL.Models
{
    public class UseMaterial
    {
        [Key]
        public int UseMaterialId { get; set; }

        // Khóa ngoại tới bảng Jobs
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        // Khóa ngoại tới bảng Materials
        public int MaterialId { get; set; }
        [ForeignKey("MaterialId")]
        public virtual Material Material { get; set; }

        public double Quantity { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }
    }
}