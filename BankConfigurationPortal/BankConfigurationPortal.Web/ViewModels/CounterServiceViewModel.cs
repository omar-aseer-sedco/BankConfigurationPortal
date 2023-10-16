using BankConfigurationPortal.Data.Models;

namespace BankConfigurationPortal.Web.ViewModels {
    public class CounterServiceViewModel {
        public BankService Service { get; set; }
        public int BranchId { get; set; }
        public int CounterId { get; set; }
        public bool IsAvailableOnCounter { get; set; }
        public bool Selected { get; set; }
    }
}