using BankConfigurationPortal.Data.Constants;
using BankConfigurationPortal.Data.Models;
using BankConfigurationPortal.Data.Utils;
using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BankConfigurationPortal.Data.Services {
    public class SqlBankServiceData : IBankServiceData {
        public IEnumerable<BankService> GetAllBankServices(string bankName) {
            try {
                string query = $"SELECT * FROM {BankServicesConstants.TABLE_NAME} WHERE {BankServicesConstants.BANK_NAME} = @bankName;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BankServicesConstants.BANK_NAME_SIZE).Value = bankName;

                List<BankService> bankServices = new List<BankService>();
                DbUtils.ExecuteReader(command, (reader) => {
                    while (reader.Read()) {
                        int bankServiceId = (int) reader[BankServicesConstants.BANK_SERVICE_ID];
                        string nameEn = (string) reader[BankServicesConstants.NAME_EN];
                        string nameAr = (string) reader[BankServicesConstants.NAME_AR];
                        bool active = (bool) reader[BankServicesConstants.ACTIVE];
                        int maxDailyTickets = (int) reader[BankServicesConstants.MAX_DAILY_TICKETS];

                        bankServices.Add(new BankService() {
                            BankName = bankName,
                            BankServiceId = bankServiceId,
                            NameEn = nameEn,
                            NameAr = nameAr,
                            Active = active,
                            MaxDailyTickets = maxDailyTickets,
                        });
                    }

                    reader.Close();
                });

                return bankServices;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public BankService GetBankService(string bankName, int bankServiceId) {
            try {
                string query = $"SELECT * FROM {BankServicesConstants.TABLE_NAME} WHERE {BankServicesConstants.BANK_NAME} = @bankName AND {BankServicesConstants.BANK_SERVICE_ID} = @bankServiceId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BankServicesConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@bankServiceId", SqlDbType.Int).Value = bankServiceId;

                BankService bankService = null;
                DbUtils.ExecuteReader(command, (reader) => {
                    if (reader.Read()) {
                        string nameEn = (string) reader[BankServicesConstants.NAME_EN];
                        string nameAr = (string) reader[BankServicesConstants.NAME_AR];
                        bool active = (bool) reader[BankServicesConstants.ACTIVE];
                        int maxDailyTickets = (int) reader[BankServicesConstants.MAX_DAILY_TICKETS];

                        bankService = new BankService() {
                            BankName = bankName,
                            BankServiceId = bankServiceId,
                            NameEn = nameEn,
                            NameAr = nameAr,
                            Active = active,
                            MaxDailyTickets = maxDailyTickets,
                        };
                    }

                    reader.Close();
                });

                return bankService;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int Add(BankService bankService) {
            try {
                string query = $"INSERT INTO {BankServicesConstants.TABLE_NAME} ({BankServicesConstants.BANK_NAME}, {BankServicesConstants.NAME_EN}, {BankServicesConstants.NAME_AR}, {BankServicesConstants.ACTIVE}, " +
                               $"{BankServicesConstants.MAX_DAILY_TICKETS}) VALUES (@bankName, @nameEn, @nameAr, @active, @maxDailyTickets); SELECT CAST(IDENT_CURRENT('{BankServicesConstants.TABLE_NAME}') AS INT);";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BankServicesConstants.BANK_NAME_SIZE).Value = bankService.BankName;
                command.Parameters.Add("@nameEn", SqlDbType.VarChar, BankServicesConstants.NAME_EN_SIZE).Value = bankService.NameEn;
                command.Parameters.Add("@nameAr", SqlDbType.NVarChar, BankServicesConstants.NAME_AR_SIZE).Value = bankService.NameAr;
                command.Parameters.Add("@active", SqlDbType.Bit).Value = bankService.Active;
                command.Parameters.Add("@maxDailyTickets", SqlDbType.Int).Value = bankService.MaxDailyTickets;

                return (int) DbUtils.ExecuteScalar(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int Update(BankService bankService) {
            try {
                string query = $"UPDATE {BankServicesConstants.TABLE_NAME} SET {BankServicesConstants.NAME_EN} = @nameEn, {BankServicesConstants.NAME_AR} = @nameAr, {BankServicesConstants.ACTIVE} = @active, {BankServicesConstants.MAX_DAILY_TICKETS} = @maxDailyTickets WHERE {BankServicesConstants.BANK_NAME} = @bankName AND {BankServicesConstants.BANK_SERVICE_ID} = @bankServiceId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@nameEn", SqlDbType.VarChar, BankServicesConstants.NAME_EN_SIZE).Value = bankService.NameEn;
                command.Parameters.Add("@nameAr", SqlDbType.NVarChar, BankServicesConstants.NAME_AR_SIZE).Value = bankService.NameAr;
                command.Parameters.Add("@active", SqlDbType.Bit).Value = bankService.Active;
                command.Parameters.Add("@maxDailyTickets", SqlDbType.Int).Value = bankService.MaxDailyTickets;
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BankServicesConstants.BANK_NAME_SIZE).Value = bankService.BankName;
                command.Parameters.Add("@bankServiceId", SqlDbType.Int).Value = bankService.BankServiceId;

                return DbUtils.ExecuteNonQuery(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int Delete(string bankName, int bankServiceId) {
            return Delete(bankName, new List<int>() { bankServiceId });
        }

        public int Delete(string bankName, IEnumerable<int> bankServiceIds) {
            try {
                SqlCommand command = new SqlCommand();
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BankServicesConstants.BANK_NAME_SIZE).Value = bankName;

                var query = new StringBuilder($"DELETE FROM {BankServicesConstants.TABLE_NAME} WHERE {BankServicesConstants.BANK_NAME} = @bankName AND {BankServicesConstants.BANK_SERVICE_ID} IN (");

                int i = 0;
                foreach (int id in bankServiceIds) {
                    query.Append("@bankServiceId").Append(i).Append(',');
                    command.Parameters.Add("@bankServiceId" + i, SqlDbType.Int).Value = id;
                    ++i;
                }

                query.Length--;
                query.Append(");");

                command.CommandText = query.ToString();

                return DbUtils.ExecuteNonQuery(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }
    }
}
