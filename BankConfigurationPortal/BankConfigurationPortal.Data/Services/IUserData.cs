using BankConfigurationPortal.Data.Models;

namespace BankConfigurationPortal.Data.Services {
    public interface IUserData {
        User GetUser(string username);
        bool ValidateUser(User user);
    }
}
