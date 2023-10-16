using BankConfigurationPortal.Web.Models;
using System.Collections.Generic;

namespace BankConfigurationPortal.Web.Services {
    public interface IBranchData {
        IEnumerable<Branch> GetAllBranches(string bankName);
        Branch GetBranch(string bankName, int branchId);
        IEnumerable<Counter> GetCountersWithoutServices(string bankName, int branchId);
        int GetNumberOfCounters(string bankName, int branchId);
        int Add(Branch branch);
        int Update(Branch branch);
        int Delete(string bankName, int branchId);
        int Delete(string bankName, IEnumerable<int> branchIds);
    }
}
