using BankConfigurationPortal.Web.Models;
using System.Collections.Generic;

namespace BankConfigurationPortal.Web.Services {
    public interface ICounterData {
        IEnumerable<Counter> GetAllCountersWithoutServices(string bankName, int branchId);
        Counter GetCounter(string bankName, int branchId, int counterId);
        IEnumerable<BankService> GetServices(string bankName, int branchId, int counterId);
        int Add(Counter counter);
        int Update(Counter counter);
        int Delete(string bankName, int branchId, int counterId);
        int Delete(string bankName, int branchId, IEnumerable<int> counterIds);
        bool CheckIfBranchExists(string bankName, int branchId);
    }
}
