using BankConfigurationPortal.Web.Constants;
using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Web.Models {
    public class Branch {
        [MaxLength(BranchesConstants.BANK_NAME_SIZE, ErrorMessageResourceName = "MaxLengthValidationMessage255", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "BankName", ResourceType = typeof(WebResources))]
        public string BankName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "BranchId", ResourceType = typeof(WebResources))]
        public int BranchId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [MaxLength(BranchesConstants.NAME_EN_SIZE, ErrorMessageResourceName = "MaxLengthValidationMessage100", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "NameEn", ResourceType = typeof(WebResources))]
        public string NameEn { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [MaxLength(BranchesConstants.NAME_AR_SIZE, ErrorMessageResourceName = "MaxLengthValidationMessage100", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "NameAr", ResourceType = typeof(WebResources))]
        public string NameAr { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "Active", ResourceType = typeof(WebResources))]
        public bool Active { get; set; }
    }
}
