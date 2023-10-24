using BankConfigurationPortal.Web.Models;

namespace BankConfigurationPortal.Web.Services {
    public interface IUserData {
        User GetUser(string username);
        bool ValidateUser(User user);
        Session GetSession(int sessionId);
        int SetSession(Session session);
        int DeleteSession(int sessionId);
    }
}
