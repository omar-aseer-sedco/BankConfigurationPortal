namespace BankConfigurationPortal.Web.Models {
    public class IssueTicketButton : TicketingButton {
        public int ServiceId { get; set; }
        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }
    }
}
