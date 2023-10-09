using BankConfigurationPortal.Data.Models;
using System;
using System.Collections.Generic;

namespace BankConfigurationPortal.Data.Services {
    class CounterData {
        public IEnumerable<Counter> GetAllCounters(string bankName, int branchId) {
            throw new NotImplementedException();
        }

        public Counter GetCounter(string bankName, int branchId, int counterId) {
            throw new NotImplementedException();
        }

        public IEnumerable<BankService> GetServices(string bankName, int branchId, int counterId) {
            throw new NotImplementedException();
        }

        public void Add(Counter counter) {
            throw new NotImplementedException();
        }

        public void Add(IEnumerable<Counter> counters) {
            throw new NotImplementedException();
        }

        public void Update(Counter counter) {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<Counter> counters) {
            throw new NotImplementedException();
        }

        public void Delete(string bankName, int branchId, int counterId) {
            throw new NotImplementedException();
        }

        public void Delete(string bankName, int branchId, IEnumerable<int> counterIds) {
            throw new NotImplementedException();
        }
    }
}
