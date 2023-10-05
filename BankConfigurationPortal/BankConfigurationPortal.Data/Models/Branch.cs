using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Data.Models {
    public class Branch {
        [Required]
        [MaxLength(255)]
        public string BankName { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameEn { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameAr { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
