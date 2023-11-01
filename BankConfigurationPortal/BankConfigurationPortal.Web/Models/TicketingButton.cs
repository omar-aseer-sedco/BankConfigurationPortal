using BankConfigurationPortal.Web.Constants;

namespace BankConfigurationPortal.Web.Models {
    public class TicketingButton {
        public string BankName { get; set; }
        public int ScreenId { get; set; }
        public int ButtonId { get; set; }
        public ButtonsConstants.Types Type { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
