using BankConfigurationPortal.Web.Models;

namespace BankConfigurationPortal.Web.ViewModels {
    public class ServiceViewModel : BankService {
        public bool IsAvailableOnCounter { get; set; }

        public ServiceViewModel(BankService service) {
            BankName = service.BankName;
            BankServiceId = service.BankServiceId;
            NameEn = service.NameEn;
            NameAr = service.NameAr;
            Active = service.Active;
            MaxDailyTickets = service.MaxDailyTickets;
            MinServiceTime = service.MinServiceTime;
            MaxServiceTime = service.MaxServiceTime;
        }
    }
}