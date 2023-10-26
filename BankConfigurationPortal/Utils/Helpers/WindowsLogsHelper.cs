using System;
using System.Diagnostics;

namespace BankConfigurationPortal.Utils.Helpers {
    public static class WindowsLogsHelper {
        private static readonly string eventLogName = "BankConfigurationPortal";
        private static readonly EventLog eventLog = new EventLog(eventLogName) {
            Source = eventLogName,
        };

        public static bool IsLogSourceInitialized() {
            return EventLog.Exists(eventLogName);
        }

        public static void Log(string message, EventLogEntryType type) {
            try {
                if (IsLogSourceInitialized()) {
                    eventLog.WriteEntry(message, type);
                }
                else {
                    throw new Exception("Log source not initialized.");
                }
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
