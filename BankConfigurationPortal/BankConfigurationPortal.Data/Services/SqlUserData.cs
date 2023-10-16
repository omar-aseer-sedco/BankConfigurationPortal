using BankConfigurationPortal.Data.Constants;
using BankConfigurationPortal.Data.Models;
using BankConfigurationPortal.Data.Utils;
using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BankConfigurationPortal.Data.Services {
    public class SqlUserData : IUserData {
        public User GetUser(string username) {
            try {
                string query = $"SELECT * FROM {UsersConstants.TABLE_NAME} WHERE {UsersConstants.USERNAME} = @username;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@username", SqlDbType.VarChar, UsersConstants.USERNAME_SIZE).Value = username;

                User user = new User();
                DbUtils.ExecuteReader(command, (reader) => {
                    if (reader.Read()) {
                        user.Username = username;
                        user.Password = (string) reader[UsersConstants.PASSWORD];
                        user.BankName = (string) reader[UsersConstants.BANK_NAME];
                    }

                    reader.Close();
                });

                return user;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public bool ValidateUser(User user) {
            try {
                var _user = GetUser(user.Username);
                if (_user.Username == string.Empty)
                    return false;

                return _user.Password == user.Password && _user.BankName.ToLower() == user.BankName.ToLower();
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }
    }
}
