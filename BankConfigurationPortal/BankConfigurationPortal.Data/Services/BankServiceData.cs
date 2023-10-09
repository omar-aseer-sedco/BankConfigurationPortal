using BankConfigurationPortal.Data.Models;
using System;
using System.Collections.Generic;

namespace BankConfigurationPortal.Data.Services {
    class BankServiceData {
        public IEnumerable<BankService> GetAllBankServices(string bankName) {
            throw new NotImplementedException();
        }

        public BankService GetBankService(string bankName, int serviceId) {
            throw new NotImplementedException();
        }

        public void Add(BankService bankService) {
            throw new NotImplementedException();
        }

        public void Add(IEnumerable<BankService> bankServices) {
            throw new NotImplementedException();
        }

        public void Update(BankService bankService) {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<BankService> bankServices) {
            throw new NotImplementedException();
        }

        public void Delete(string bankName, int serviceId) {
            throw new NotImplementedException();
        }

        public void Delete(string bankName, IEnumerable<int> serviceIds) {
            throw new NotImplementedException();
        }
    }
}
