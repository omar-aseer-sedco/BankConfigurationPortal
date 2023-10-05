using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Data.Models {
    public class Bank {
        [Required]
        [MaxLength(255)]
        public string BankName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
