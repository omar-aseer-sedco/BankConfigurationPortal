using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Utils;
using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace BankConfigurationPortal.Web.Services {
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

        public Session GetSession(int sessionId) {
            try {
                string query = $"SELECT * FROM {SessionsConstants.TABLE_NAME} WHERE {SessionsConstants.SESSION_ID} = @sessionId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@sessionId", SqlDbType.Int).Value = sessionId;

                Session session = new Session();
                DbUtils.ExecuteReader(command, (reader) => {
                    if (reader.Read()) {
                        session.Username = (string) reader[SessionsConstants.USERNAME];
                        session.SessionId = sessionId;
                        session.Expires = (DateTime) reader[SessionsConstants.EXPIRES];
                        session.UserAgent = (string) reader[SessionsConstants.USER_AGENT];
                        session.IpAddress = (string) reader[SessionsConstants.IP_ADDRESS];
                    }

                    reader.Close();
                });

                return session;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int SetSession(Session session) {
            try {
                if (GetSession(session.SessionId).Username == default) {
                    return AddSession(session);
                }
                else {
                    return UpdateSession(session);
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        private int AddSession(Session session) {
            try {
                string query = $"INSERT INTO {SessionsConstants.TABLE_NAME} ({SessionsConstants.USERNAME}, {SessionsConstants.SESSION_ID}, {SessionsConstants.EXPIRES}, " +
                               $"{SessionsConstants.USER_AGENT}, {SessionsConstants.IP_ADDRESS}) VALUES (@username, @sessionId, @expires, @userAgent, @ipAddress);";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@username", SqlDbType.VarChar, SessionsConstants.USERNAME_SIZE).Value = session.Username;
                command.Parameters.Add("@sessionId", SqlDbType.Int).Value = session.SessionId;
                command.Parameters.Add("@expires", SqlDbType.DateTime).Value = (SqlDateTime) session.Expires;
                command.Parameters.Add("@userAgent", SqlDbType.VarChar, SessionsConstants.USER_AGENT_SIZE).Value = session.UserAgent;
                command.Parameters.Add("@ipAddress", SqlDbType.VarChar, SessionsConstants.IP_ADDRESS_SIZE).Value = session.IpAddress;

                return DbUtils.ExecuteNonQuery(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        private int UpdateSession(Session session) {
            try {
                string query = $"UPDATE {SessionsConstants.TABLE_NAME} SET {SessionsConstants.EXPIRES} = @expires, {SessionsConstants.USER_AGENT} = @userAgent, " +
                               $"{SessionsConstants.IP_ADDRESS} = @ipAddress WHERE {SessionsConstants.SESSION_ID} = @sessionId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@expires", SqlDbType.DateTime).Value = (SqlDateTime) session.Expires;
                command.Parameters.Add("@userAgent", SqlDbType.VarChar, SessionsConstants.USER_AGENT_SIZE).Value = session.UserAgent;
                command.Parameters.Add("@ipAddress", SqlDbType.VarChar, SessionsConstants.IP_ADDRESS_SIZE).Value = session.IpAddress;
                command.Parameters.Add("@sessionId", SqlDbType.Int).Value = session.SessionId;

                return DbUtils.ExecuteNonQuery(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int DeleteSession(int sessionId) {
            try {
                string query = $"DELETE FROM {SessionsConstants.TABLE_NAME} WHERE {SessionsConstants.SESSION_ID} = @sessionId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@sessionId", SqlDbType.Int).Value = sessionId;

                return DbUtils.ExecuteNonQuery(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }
    }
}
