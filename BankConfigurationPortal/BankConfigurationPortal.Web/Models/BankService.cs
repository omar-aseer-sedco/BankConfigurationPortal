using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Constants;
using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Web.Models {
    public class BankService {
        [MaxLength(BankServicesConstants.BANK_NAME_SIZE, ErrorMessageResourceName = "MaxLengthValidationMessage255", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "BankName", ResourceType = typeof(WebResources))]
        public string BankName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "BankServiceId", ResourceType = typeof(WebResources))]
        public int BankServiceId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [MaxLength(BankServicesConstants.NAME_EN_SIZE, ErrorMessageResourceName = "MaxLengthValidationMessage100", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "NameEn", ResourceType = typeof(WebResources))]
        public string NameEn { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [MaxLength(BankServicesConstants.NAME_AR_SIZE, ErrorMessageResourceName = "MaxLengthValidationMessage100", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "NameAr", ResourceType = typeof(WebResources))]
        public string NameAr { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "Active", ResourceType = typeof(WebResources))]
        public bool Active { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [Range(1, 100, ErrorMessageResourceName = "RangeValidationMessage1_100", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "MaxDailyTickets", ResourceType = typeof(WebResources))]
        public int MaxDailyTickets { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [Range(30, 999999, ErrorMessageResourceName = "RangeValidationMessage30_999999", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "MinServiceTime", ResourceType = typeof(WebResources))]
        [MinServiceTimeValidation(nameof(MaxServiceTime), ErrorMessageResourceName = "MinServiceTimeValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        public int MinServiceTime { get; set; } = 45;

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [Range(30, 999999, ErrorMessageResourceName = "RangeValidationMessage30_999999", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "MaxServiceTime", ResourceType = typeof(WebResources))]
        [MaxServiceTimeValidation(nameof(MinServiceTime), ErrorMessageResourceName = "MaxServiceTimeValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        public int MaxServiceTime { get; set; } = 300;
    }
}
