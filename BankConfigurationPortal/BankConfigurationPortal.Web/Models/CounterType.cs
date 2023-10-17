using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Web.Models {
    public enum CounterType {
        [Display(Name = "Teller", ResourceType = typeof(WebResources))]
        Teller = 1,
        [Display(Name = "CustomerService", ResourceType = typeof(WebResources))]
        CustomerService,
    }
}
