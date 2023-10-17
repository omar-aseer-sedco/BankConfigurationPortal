using BankConfigurationPortal.Web.Models;

namespace BankConfigurationPortal.Web.ViewModels {
    public class CounterServiceViewModel {
        public BankService Service { get; set; }
        public bool IsAvailableOnCounter { get; set; }
    }
}