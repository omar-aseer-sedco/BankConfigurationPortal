using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Data.Models { 
    public class Branch {
        //[Required] idk why this isn't working but whatever for now
        [MaxLength(255)]
        [Display(Name = "BankName", ResourceType = typeof(WebResources))]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "BranchId", ResourceType = typeof(WebResources))]
        public int BranchId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "NameEn", ResourceType = typeof(WebResources))]
        public string NameEn { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "NameAr", ResourceType = typeof(WebResources))]
        public string NameAr { get; set; }

        [Required]
        [Display(Name = "Active", ResourceType = typeof(WebResources))]
        public bool Active { get; set; }
    }
}
