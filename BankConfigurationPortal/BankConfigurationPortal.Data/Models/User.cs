using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Data.Models {
    public class User {
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string BankName { get; set; }
    }
}
