using System;
using System.Data.SqlClient;

namespace BankConfigurationPortal.Utils.Helpers {
    public static class ExceptionHelper {
        public static void HandleGeneralException(Exception exception) {
            string message = $"Unhandled Error.\nType: {exception.GetType()}\nMessage: {exception.Message}";
            LogsHelper.Log(new LogEvent(message, DateTime.Now, EventSeverity.Error, exception.Source, exception.StackTrace));
        }

        public static void HandleSqlException(SqlException exception) {
            // temporary implementation
            HandleGeneralException(exception);
        }
    }
}
