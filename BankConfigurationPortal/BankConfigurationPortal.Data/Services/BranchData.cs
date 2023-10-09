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
    class BranchData {
        public IEnumerable<Branch> GetAllBranches(string bankName) {
            try {
                string query = $"SELECT * FROM {BranchesConstants.TABLE_NAME} WHERE {BranchesConstants.BANK_NAME} = @bankName;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BranchesConstants.BANK_NAME_SIZE).Value = bankName;

                List<Branch> branches = new List<Branch>();
                DbUtils.ExecuteReader(command, (reader) => {
                    while (reader.Read()) {
                        int branchId = (int) reader[BranchesConstants.BRANCH_ID];
                        string nameEn = (string) reader[BranchesConstants.NAME_EN];
                        string nameAr = (string) reader[BranchesConstants.NAME_AR];
                        bool active = (bool) reader[BranchesConstants.ACTIVE];

                        branches.Add(new Branch() {
                            BankName = bankName,
                            BranchId = branchId,
                            NameEn = nameEn,
                            NameAr = nameAr,
                            Active = active,
                        });
                    }

                    reader.Close();
                });

                return branches;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public Branch GetBranch(string bankName, int branchId) {
            try {
                string query = $"SELECT * FROM {BranchesConstants.TABLE_NAME} WHERE {BranchesConstants.BANK_NAME} = @bankName AND {BranchesConstants.BRANCH_ID} = @branchId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BranchesConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branchId;

                Branch branch = new Branch();
                DbUtils.ExecuteReader(command, (reader) => {
                    if (reader.Read()) {
                        string nameEn = (string) reader[BranchesConstants.NAME_EN];
                        string nameAr = (string) reader[BranchesConstants.NAME_AR];
                        bool active = (bool) reader[BranchesConstants.ACTIVE];

                        branch = new Branch() {
                            BankName = bankName,
                            BranchId = branchId,
                            NameEn = nameEn,
                            NameAr = nameAr,
                            Active = active,
                        };
                    }

                    reader.Close();
                });

                return branch;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public IEnumerable<Counter> GetCountersWithoutServices(string bankName, int branchId) {
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
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int Add(Branch branch) {
            try {
                string query = $"INSERT INTO {BranchesConstants.TABLE_NAME} ({BranchesConstants.BANK_NAME}, {BranchesConstants.NAME_EN}, {BranchesConstants.NAME_AR}, {BranchesConstants.ACTIVE}) " +
                               $"VALUES (@bankName, @nameEn, @nameAr, @active); SELECT CAST(IDENT_CURRENT('{BranchesConstants.TABLE_NAME}') AS INT);";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BranchesConstants.BANK_NAME_SIZE).Value = branch.BankName;
                command.Parameters.Add("@nameEn", SqlDbType.VarChar, BranchesConstants.NAME_EN_SIZE).Value = branch.NameEn;
                command.Parameters.Add("@nameAr", SqlDbType.NVarChar, BranchesConstants.NAME_AR_SIZE).Value = branch.NameAr;
                command.Parameters.Add("@active", SqlDbType.Bit).Value = branch.Active;

                return (int) DbUtils.ExecuteScalar(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int Update(Branch branch) {
            try {
                string query = $"UPDATE {BranchesConstants.TABLE_NAME} SET {BranchesConstants.NAME_EN} = @nameEn, {BranchesConstants.NAME_AR} = @nameAr, {BranchesConstants.ACTIVE} = @active WHERE {BranchesConstants.BANK_NAME} = @bankName AND {BranchesConstants.BRANCH_ID} = @branchId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@nameEn", SqlDbType.VarChar, BranchesConstants.NAME_EN_SIZE).Value = branch.NameEn;
                command.Parameters.Add("@nameAr", SqlDbType.NVarChar, BranchesConstants.NAME_AR_SIZE).Value = branch.NameAr;
                command.Parameters.Add("@active", SqlDbType.Bit).Value = branch.Active;
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BranchesConstants.BANK_NAME_SIZE).Value = branch.BankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branch.BranchId;

                return DbUtils.ExecuteNonQuery(command);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public int Delete(string bankName, int branchId) {
            return Delete(bankName, new List<int>() { branchId });
        }

        public int Delete(string bankName, IEnumerable<int> branchIds) {
            try {
                SqlCommand command = new SqlCommand();
                command.Parameters.Add("@bankName", SqlDbType.VarChar, BranchesConstants.BANK_NAME_SIZE).Value = bankName;

                var query = new StringBuilder($"DELETE FROM {BranchesConstants.TABLE_NAME} WHERE {BranchesConstants.BANK_NAME} = @bankName AND {BranchesConstants.BRANCH_ID} IN (");

                int i = 0;
                foreach (int id in branchIds) {
                    query.Append("@branchId").Append(i).Append(',');
                    command.Parameters.Add("@branchId" + i, SqlDbType.Int).Value = id;
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
