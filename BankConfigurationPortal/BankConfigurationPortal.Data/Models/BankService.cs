using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Data.Models {
    public class BankService {
        //[Required]
        [MaxLength(255)]
        [Display(Name = "BankName", ResourceType = typeof(WebResources))]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "BankServiceId", ResourceType = typeof(WebResources))]
        public int BankServiceId { get; set; }

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
        [Range(1, 100)]
        [Display(Name = "MaxDailyTickets", ResourceType = typeof(WebResources))]
        public int MaxDailyTickets { get; set; }
    }
}
