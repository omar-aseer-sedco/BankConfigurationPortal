using System;
using System.IO;
using System.Text.Json;

namespace BankConfigurationPortal.Utils.Helpers {
    public enum EventSeverity {
        None,
        Error,
        Warning,
        Info,
    }

    public class LogEvent {
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Severity { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }

        public LogEvent() {
            Message = string.Empty;
            TimeStamp = DateTime.Now;
            Severity = Enum.GetName(typeof(EventSeverity), EventSeverity.None);
            Source = string.Empty;
            StackTrace = string.Empty;
        }

        public LogEvent(string message, DateTime timeStamp, EventSeverity severity) : this() {
            Message = message;
            TimeStamp = timeStamp;
            Severity = Enum.GetName(typeof(EventSeverity), severity);
        }

        public LogEvent(string message, DateTime timeStamp, EventSeverity severity, string source, string stackTrace) : this(message, timeStamp, severity) {
            Source = source;
            StackTrace = stackTrace;
        }
    }

    public static class LogsHelper {
        private static readonly string logsDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private static readonly string logsFilePath = Path.Combine(logsDirectoryPath, "logs.json");

        private static void InitializeFile() {
            try {
                if (!File.Exists(logsFilePath)) {
                    if (!Directory.Exists(logsDirectoryPath)) {
                        Directory.CreateDirectory(logsDirectoryPath);
                    }

                    using (var fileWriter = File.AppendText(logsFilePath)) {
                        fileWriter.Write("[");
                    }
                }
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public static void Log(LogEvent logEvent) {
            try {
                InitializeFile();

                var options = new JsonSerializerOptions() {
                    WriteIndented = true,
                };

                bool isFirstLog = true;
                using (var fileStream = new FileStream(logsFilePath, FileMode.Open, FileAccess.ReadWrite)) {
                    fileStream.Seek(1, SeekOrigin.Begin);
                    if (fileStream.ReadByte() == '{') {
                        isFirstLog = false;

                        fileStream.Seek(-3, SeekOrigin.End);
                        if (fileStream.ReadByte() == ']') {
                            fileStream.SetLength(fileStream.Length - 3);
                        }
                    }
                }

                using (var fileWriter = File.AppendText(logsFilePath)) {
                    fileWriter.Write((isFirstLog ? "" : ",\n") + JsonSerializer.Serialize(logEvent, typeof(LogEvent), options));
                    fileWriter.WriteLine("]");
                }
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public static void Log(string message, EventSeverity severity) {
            Log(new LogEvent(message, DateTime.Now, severity, string.Empty, string.Empty));
        }
    }
}
