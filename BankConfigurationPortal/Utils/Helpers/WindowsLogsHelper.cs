using System;
using System.Diagnostics;

namespace Utils.Helpers {
    public static class WindowsLogsHelper {
        private static readonly string eventLogName = "BankConfigurationPortal";
        private static readonly EventLog eventLog = new EventLog(eventLogName);

        public static bool IsLogSourceInitialized() {
            return EventLog.Exists(eventLogName);
        }

        public static void Log(string message, EventLogEntryType type) {
            try {
                if (IsLogSourceInitialized()) {
                    eventLog.WriteEntry(message, type);
                }
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
