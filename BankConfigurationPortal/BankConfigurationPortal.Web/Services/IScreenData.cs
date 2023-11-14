using BankConfigurationPortal.Web.Models;
using System.Collections.Generic;

namespace BankConfigurationPortal.Web.Services {
    public interface IScreenData {
        TicketingScreen GetActiveScreen(string bankName);
        IEnumerable<TicketingButton> GetButtons(string bankName, int screenId);
        IEnumerable<TicketingButton> GetButtons(string bankName, int screenId, int branchId);
        IEnumerable<TicketingButton> GetButtons(string bankName, int screenId, int branchId, int counterId);
        bool CheckIfBranchExists(string bankName, int branchId);
        bool CheckIfCounterExists(string bankName, int branchid, int counterId);
        CounterInformation GetCounterInformation(string bankName, int branchId, int counterId);
    }
}
