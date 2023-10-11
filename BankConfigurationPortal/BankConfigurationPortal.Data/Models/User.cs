using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Data.Models {
    public class User {
        [Required]
        [MaxLength(255)]
        [Display(Name = "Username", ResourceType = typeof(WebResources))]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Password", ResourceType = typeof(WebResources))]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "BankName", ResourceType = typeof(WebResources))]
        public string BankName { get; set; }
    }
}
