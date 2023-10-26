using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BankConfigurationPortal.Utils.Helpers {
    public static class ExceptionHelper {
        public static void HandleGeneralException(Exception exception) {
            string message = $"Unhandled Error.\nType: {exception.GetType()}\nMessage: {exception.Message}\nSource: {exception.Source}\nStackTrace: {exception.StackTrace}\n";
            WindowsLogsHelper.Log(message, EventLogEntryType.Error);
        }

        public static void HandleSqlException(SqlException exception) {
            foreach (SqlError error in exception.Errors) {
                string message = $"Unhandled SQL Error. Code: {error.Number}\nMessage: {error.Message}\nSource: {error.Source}\nStackTrace: {exception.StackTrace}\n";
                WindowsLogsHelper.Log(message, EventLogEntryType.Error);
            }
        }
    }
}
