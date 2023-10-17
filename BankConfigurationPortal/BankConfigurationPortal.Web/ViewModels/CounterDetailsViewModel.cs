using BankConfigurationPortal.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankConfigurationPortal.Web.ViewModels {
    public class CounterDetailsViewModel {
        [Display(Name = "BranchId", ResourceType = typeof(WebResources))]
        public int BranchId { get; set; }

        [Display(Name = "CounterId", ResourceType = typeof(WebResources))]
        public int CounterId { get; set; }

        [Display(Name = "NameEn", ResourceType = typeof(WebResources))]
        public string NameEn { get; set; }

        [Display(Name = "NameAr", ResourceType = typeof(WebResources))]
        public string NameAr { get; set; }

        [Display(Name = "Active", ResourceType = typeof(WebResources))]
        public bool Active { get; set; }

        [Display(Name = "CounterType", ResourceType = typeof(WebResources))]
        public CounterType Type { get; set; }

        [Display(Name = "Services", ResourceType = typeof(WebResources))]
        public IEnumerable<CounterServiceViewModel> Services { get; set; }
    }
}
