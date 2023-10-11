using BankConfigurationPortal.Data.Models;
using System.Collections.Generic;

namespace BankConfigurationPortal.Data.Services {
    public interface IBankServiceData {
        IEnumerable<BankService> GetAllBankServices(string bankName);
        BankService GetBankService(string bankName, int bankServiceId);
        int Add(BankService bankService);
        int Update(BankService bankService);
        int Delete(string bankName, int bankServiceId);
        int Delete(string bankName, IEnumerable<int> bankServiceIds);
    }
}
