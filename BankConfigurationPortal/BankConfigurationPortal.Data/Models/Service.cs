using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Data.Models {
    public class Service {
        [Required]
        [MaxLength(255)]
        public string BankName { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameEn { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameAr { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        [Range(1, 100)]
        public int MaxDailyTickets { get; set; }
    }
}
