using BankConfigurationPortal.Web.Constants;
using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Web.Models {
    public class User {
        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [MaxLength(UsersConstants.USERNAME_SIZE, ErrorMessageResourceName = "MaxLengthValidationMessage255", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "Username", ResourceType = typeof(WebResources))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [MaxLength(UsersConstants.PASSWORD_SIZE, ErrorMessageResourceName = "MaxLengthValidationMessage255", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "Password", ResourceType = typeof(WebResources))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMessage", ErrorMessageResourceType = typeof(WebResources))]
        [MaxLength(UsersConstants.BANK_NAME_SIZE, ErrorMessageResourceName = "MaxLengthValidationMessage255", ErrorMessageResourceType = typeof(WebResources))]
        [Display(Name = "BankName", ResourceType = typeof(WebResources))]
        public string BankName { get; set; }
    }
}
