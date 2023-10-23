using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Utils;
using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BankConfigurationPortal.Web.Services {
    public class SqlCounterData : ICounterData {
        public IEnumerable<Counter> GetAllCountersWithoutServices(string bankName, int branchId) {
            try {
                string query = $"SELECT * FROM {CountersConstants.TABLE_NAME} WHERE {CountersConstants.BANK_NAME} = @bankName AND {CountersConstants.BRANCH_ID} = @branchId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BranchesConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branchId;

                List<Counter> counters = new List<Counter>();
                DbUtils.ExecuteReader(command, (reader) => {
                    while (reader.Read()) {
                        int counterId = (int) reader[CountersConstants.COUNTER_ID];
                        string nameEn = (string) reader[CountersConstants.NAME_EN];
                        string nameAr = (string) reader[CountersConstants.NAME_AR];
                        bool active = (bool) reader[CountersConstants.ACTIVE];
                        CounterType type = (CounterType) reader[CountersConstants.TYPE];

                        counters.Add(new Counter() {
                            BankName = bankName,
                            BranchId = branchId,
                            CounterId = counterId,
                            NameEn = nameEn,
                            NameAr = nameAr,
                            Active = active,
                            Type = type,
                        });
                    }

                    reader.Close();
                });

                return counters;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public Counter GetCounter(string bankName, int branchId, int counterId) {
            try {
                string query = $"SELECT * FROM {CountersConstants.TABLE_NAME} WHERE {CountersConstants.BANK_NAME} = @bankName AND {CountersConstants.BRANCH_ID} = @branchId AND {CountersConstants.COUNTER_ID} = @counterId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BranchesConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branchId;
                command.Parameters.Add("@counterId", SqlDbType.Int).Value = counterId;

                Counter counter = null;
                DbUtils.ExecuteReader(command, (reader) => {
                    if (reader.Read()) {
                        string nameEn = (string) reader[CountersConstants.NAME_EN];
                        string nameAr = (string) reader[CountersConstants.NAME_AR];
                        bool active = (bool) reader[CountersConstants.ACTIVE];
                        CounterType type = (CounterType) reader[CountersConstants.TYPE];

                        counter = new Counter() {
                            BankName = bankName,
                            BranchId = branchId,
                            CounterId = counterId,
                            NameEn = nameEn,
                            NameAr = nameAr,
                            Active = active,
                            Type = type,
                            Services = GetServices(bankName, branchId, counterId),
                        };
                    }

                    reader.Close();
                });

                return counter;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public IEnumerable<BankService> GetServices(string bankName, int branchId, int counterId) {
            try {
                string query = $"SELECT * FROM {BankServicesConstants.TABLE_NAME} WHERE {BankServicesConstants.BANK_NAME} = @bankName AND {BankServicesConstants.BANK_SERVICE_ID} IN " +
                               $"(SELECT {ServicesCountersConstants.BANK_SERVICE_ID} FROM {ServicesCountersConstants.TABLE_NAME} WHERE {ServicesCountersConstants.BANK_NAME} = @bankName " +
                               $"AND {ServicesCountersConstants.BRANCH_ID} = @branchId AND {ServicesCountersConstants.COUNTER_ID} = @counterId);";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BranchesConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branchId;
                command.Parameters.Add("@counterId", SqlDbType.Int).Value = counterId;

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

        public int Add(Counter counter) {
            try {
                string query = $"INSERT INTO {CountersConstants.TABLE_NAME} ({CountersConstants.BANK_NAME}, {CountersConstants.BRANCH_ID}, {CountersConstants.NAME_EN}, {CountersConstants.NAME_AR}, " +
                               $"{CountersConstants.ACTIVE}, {CountersConstants.TYPE}) VALUES (@bankName, @branchId, @nameEn, @nameAr, @active, @type); SELECT CAST(IDENT_CURRENT('{CountersConstants.TABLE_NAME}') AS INT);";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, CountersConstants.BANK_NAME_SIZE).Value = counter.BankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = counter.BranchId;
                command.Parameters.Add("@nameEn", SqlDbType.VarChar, CountersConstants.NAME_EN_SIZE).Value = counter.NameEn;
                command.Parameters.Add("@nameAr", SqlDbType.NVarChar, CountersConstants.NAME_AR_SIZE).Value = counter.NameAr;
                command.Parameters.Add("@active", SqlDbType.Bit).Value = counter.Active;
                command.Parameters.Add("@type", SqlDbType.Int).Value = counter.Type;

                return (int) DbUtils.ExecuteScalar(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int Update(Counter counter) {
            try {
                string query = $"UPDATE {CountersConstants.TABLE_NAME} SET {CountersConstants.NAME_EN} = @nameEn, {CountersConstants.NAME_AR} = @nameAr, {CountersConstants.ACTIVE} = @active, {CountersConstants.TYPE} = @type WHERE {CountersConstants.BANK_NAME} = @bankName AND {CountersConstants.BRANCH_ID} = @branchId AND {CountersConstants.COUNTER_ID} = @counterId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@nameEn", SqlDbType.VarChar, CountersConstants.NAME_EN_SIZE).Value = counter.NameEn;
                command.Parameters.Add("@nameAr", SqlDbType.NVarChar, CountersConstants.NAME_AR_SIZE).Value = counter.NameAr;
                command.Parameters.Add("@active", SqlDbType.Bit).Value = counter.Active;
                command.Parameters.Add("@type", SqlDbType.Int).Value = counter.Type;
                command.Parameters.Add("@bankName", SqlDbType.VarChar, CountersConstants.BANK_NAME_SIZE).Value = counter.BankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = counter.BranchId;
                command.Parameters.Add("@counterId", SqlDbType.Int).Value = counter.CounterId;

                return DbUtils.ExecuteNonQuery(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int Delete(string bankName, int branchId, int counterId) {
            return Delete(bankName, branchId, new List<int>() { counterId });
        }

        public int Delete(string bankName, int branchId, IEnumerable<int> counterIds) {
            try {
                SqlCommand command = new SqlCommand();
                command.Parameters.Add("@bankName", SqlDbType.VarChar, CountersConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branchId;

                var query = new StringBuilder($"DELETE FROM {CountersConstants.TABLE_NAME} WHERE {CountersConstants.BANK_NAME} = @bankName AND {CountersConstants.BRANCH_ID} = @branchId AND {CountersConstants.COUNTER_ID} IN (");

                int i = 0;
                foreach (int id in counterIds) {
                    query.Append("@counterId").Append(i).Append(',');
                    command.Parameters.Add("@counterId" + i, SqlDbType.Int).Value = id;
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

        public bool CheckIfBranchExists(string bankName, int branchId) {
            try {
                string query = $"SELECT COUNT(*) FROM {BranchesConstants.TABLE_NAME} WHERE {BranchesConstants.BANK_NAME} = @bankName AND {BranchesConstants.BRANCH_ID} = @branchId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BranchesConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branchId;

                return (int) DbUtils.ExecuteScalar(command) == 1;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }
    }
}
