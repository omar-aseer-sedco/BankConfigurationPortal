using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Web.Models {
    public class Bank {
        [Required]
        [MaxLength(255)]
        [Display(Name = "BankName", ResourceType = typeof(WebResources))]
        public string BankName { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Password", ResourceType = typeof(WebResources))]
        public string Password { get; set; }
    }
}
