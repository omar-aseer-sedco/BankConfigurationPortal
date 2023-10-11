using BankConfigurationPortal.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankConfigurationPortal.Web.ViewModels {
    public class CounterServiceViewModel {
        public BankService Service { get; set; }
        public int BranchId { get; set; }
        public int CounterId { get; set; }
        public bool IsAvailableOnCounter { get; set; }
    }
}