using BankConfigurationPortal.Web.Models;

namespace BankConfigurationPortal.Web.Services {
    public interface IUserData {
        User GetUser(string username);
        bool ValidateUser(User user);
    }
}
