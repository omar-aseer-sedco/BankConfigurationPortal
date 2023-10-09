using BankConfigurationPortal.Utils.Constants;
using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Data.SqlClient;

namespace BankConfigurationPortal.Data.Utils {
    internal static class DbUtils {
        private static string GetConnectionString() {
            return "server=(local);database=BankConfigurationPortal;integrated security=sspi;";
        }

        /// <summary>
        /// Creates a new <c>SqlConnection</c>.
        /// </summary>
        /// <param name="status">The status of the connection creation.</param>
        /// <returns>The <c>SqlConnection</c> object.</returns>
        public static SqlConnection CreateConnection(out ConnectionStatus status) {
            try {
                string connectionString = GetConnectionString();
                var connection = new SqlConnection(connectionString);
                
                status = ConnectionStatus.SUCCESS;
                return connection;
            }
            catch (SqlException ex) {
                ExceptionHelper.HandleSqlException(ex);
                status = ConnectionStatus.FAILED_TO_CONNECT;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                status = ConnectionStatus.UNDEFINED_ERROR;
            }

            return null;
        }

        /// <summary>
        /// Calls <c>SqlCommand.ExecuteNonQuery</c> using the given command.
        /// </summary>
        /// <param name="command">The command to be executed</param>
        /// <returns>The number of rows affected by the operation. If the operation fails, -1 is returned.</returns>
        public static int ExecuteNonQuery(SqlCommand command) {
            try {
                using (var connection = CreateConnection(out var status)) {
                    if (status != ConnectionStatus.SUCCESS || connection == null) {
                        return -1;
                    }

                    command.Connection = connection;
                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) {
                ExceptionHelper.HandleSqlException(ex);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return -1;
        }

        /// <summary>
        /// Calls <c>SqlCommand.ExecuteScalar</c> using the given command.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        /// <returns>The result of the <c>ExecuteScalar</c> call. If the operation fails, <c>null</c> is returned.</returns>
        public static object ExecuteScalar(SqlCommand command) {
            try {
                using (var connection = CreateConnection(out var status)) {
                    if (status != ConnectionStatus.SUCCESS || connection is null) {
                        return null;
                    }

                    command.Connection = connection;
                    connection.Open();

                    return command.ExecuteScalar();
                }
            }
            catch (SqlException ex) {
                ExceptionHelper.HandleSqlException(ex);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return null;
        }

        public delegate void ReaderDelegate(SqlDataReader reader);

        /// <summary>
        /// Calls <c>SqlCommand.ExecuteReader</c> using the given command. The given <c>ReaderDelegate</c> is then executed on the resulting <c>SqlDataReader</c>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="readerDelegate">The delegate to be executed on the reader.</param>
        /// <returns><c>true</c> if the operation succeeds, and <c>false</c> otherwise.</returns>
        public static bool ExecuteReader(SqlCommand command, ReaderDelegate readerDelegate) {
            try {
                using (var connection = CreateConnection(out var status)) {
                    if (status != ConnectionStatus.SUCCESS || connection is null) {
                        return false;
                    }

                    command.Connection = connection;
                    connection.Open();

                    using (var reader = command.ExecuteReader()) {
                        readerDelegate(reader);
                    }

                    return true;
                }

            }
            catch (SqlException ex) {
                ExceptionHelper.HandleSqlException(ex);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return false;
        }
    }
}
