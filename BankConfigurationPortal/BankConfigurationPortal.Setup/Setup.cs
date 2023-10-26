using System;
using System.Diagnostics;
using System.Security;

namespace BankConfigurationPortalSetup {
    class Setup {
        static void Main() {
            try {
                Console.WriteLine("Initiating setup...\n");

                Console.WriteLine("Creating event log source...\n");
                bool createEventLogSuccess = CreateEventLog("BankConfigurationPortal");

                if (createEventLogSuccess) {
                    Console.WriteLine("Setup successful.\n");
                }
                else {
                    Console.WriteLine("Setup failed.\n");
                }

                Console.WriteLine("Press Q or Return to exit.\n");
                while (true) {
                    var keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Enter || keyInfo.Key == ConsoleKey.Q) {
                        break;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("An unexpected error has occurred.\n");
                Console.WriteLine(ex.Message);
            }
        }

        private static bool CreateEventLog(string eventLogName) {
            try {
                string logMessage;

                if (EventLog.Exists(eventLogName)) {
                    logMessage = "Event source already exists.";
                }
                else {
                    EventLog.CreateEventSource(eventLogName, eventLogName);
                    logMessage = "Event source created.";
                }

                Console.WriteLine(logMessage + "\n");

                EventLog eventLog = new EventLog(eventLogName) {
                    Source = eventLogName,
                };

                eventLog.WriteEntry(logMessage, EventLogEntryType.Information);

                return true;
            }
            catch (SecurityException) {
                Console.WriteLine("Insufficient permissions. Please make sure you are running this program as an administrator.\n");
            }
            catch (Exception ex) {
                Console.WriteLine("An unexpected error has occurred.\n");
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
