using System.Collections.Generic;

namespace BankConfigurationPortal.Web.ViewModels {
    public enum ServiceOperation {
        addServices,
        removeServices
    }

    public class SelectServicesViewModel {
        public IEnumerable<CounterServiceViewModel> CounterServices { get; set; }
    }
}
