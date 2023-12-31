﻿using BankConfigurationPortal.Web.Models;
using System.Collections.Generic;

namespace BankConfigurationPortal.Web.Services {
    public interface IBankServiceData {
        IEnumerable<BankService> GetAllBankServices(string bankName);
        BankService GetBankService(string bankName, int bankServiceId);
        int Add(BankService bankService);
        int Update(BankService bankService);
        int Delete(string bankName, int bankServiceId);
        int Delete(string bankName, IEnumerable<int> bankServiceIds);
        int AddService(string bankName, int branchId, int counterId, int bankServiceId);
        int AddServices(string bankName, int branchId, int counterId, IEnumerable<int> bankServiceIds);
        int RemoveService(string bankName, int branchId, int counterId, int bankServiceId);
        int RemoveServices(string bankName, int branchId, int counterId, IEnumerable<int> bankServiceIds);
        bool IsAvailableOnCounter(string bankName, int branchId, int counterId, int bankServiceId);
    }
}
