using BankConfigurationPortal.Data.Constants;
using BankConfigurationPortal.Data.Utils;
using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BankConfigurationPortal.Data.Services {
    class BankData {
        public string GetPassword(string bankName) {
            try {
                string query = $"SELECT {BanksConstants.PASSWORD} FROM {BanksConstants.TABLE_NAME} WHERE {BanksConstants.BANK_NAME} = @bankName;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BanksConstants.BANK_NAME_SIZE).Value = bankName;

                string password = (string) DbUtils.ExecuteScalar(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }
    }
}
