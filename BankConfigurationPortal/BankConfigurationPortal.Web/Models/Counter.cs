using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Web.Models {
    public class Counter {
        [MaxLength(255)]
        [Display(Name = "BankName", ResourceType = typeof(WebResources))]
        public string BankName { get; set; }

        [Display(Name = "BranchId", ResourceType = typeof(WebResources))]
        public int BranchId { get; set; }

        [Required]
        [Display(Name = "CounterId", ResourceType = typeof(WebResources))]
        public int CounterId { get; set; }

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

        [Required]
        [Display(Name = "CounterType", ResourceType = typeof(WebResources))]
        public CounterType Type { get; set; }

        [Display(Name = "Services", ResourceType = typeof(WebResources))]
        public IEnumerable<BankService> Services { get; set; }
    }
}
