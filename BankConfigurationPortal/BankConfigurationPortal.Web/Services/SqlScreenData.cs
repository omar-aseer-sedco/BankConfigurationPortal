using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BankConfigurationPortal.Web.Services {
    public class SqlScreenData : IScreenData {
        const string SERVICE_NAME_EN = "service_name_en";
        const string SERVICE_NAME_AR = "service_name_ar";

        public TicketingScreen GetActiveScreen(string bankName) {
            try {
                string query = $"SELECT * FROM {ScreensConstants.TABLE_NAME} WHERE {ScreensConstants.BANK_NAME} = @bankName AND {ScreensConstants.IS_ACTIVE} = 1;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, ScreensConstants.BANK_NAME_SIZE).Value = bankName;

                TicketingScreen screen = null;
                DbUtils.ExecuteReader(command, (reader) => {
                    if (reader.Read()) {
                        int screenId = (int) reader[ScreensConstants.SCREEN_ID];
                        string screenTitle = (string) reader[ScreensConstants.SCREEN_TITLE];

                        screen = new TicketingScreen {
                            BankName = bankName,
                            ScreenId = screenId,
                            ScreenTitle = screenTitle,
                            Active = true
                        };
                    }
                    else {
                        screen = new TicketingScreen();
                    }

                    reader.Close();
                });

                return screen;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public IEnumerable<TicketingButton> GetButtons(string bankName, int screenId) {
            try {
                string query = $"SELECT " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.BANK_NAME}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SCREEN_ID}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.BUTTON_ID}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.TYPE}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.NAME_EN}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.NAME_AR}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SERVICE_ID}, " +
                                   $"{BankServicesConstants.TABLE_NAME}.{BankServicesConstants.NAME_EN} AS {SERVICE_NAME_EN}, " +
                                   $"{BankServicesConstants.TABLE_NAME}.{BankServicesConstants.NAME_AR} AS {SERVICE_NAME_AR}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.MESSAGE_EN}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.MESSAGE_AR} " +
                               $"FROM " +
                                   $"{ButtonsConstants.TABLE_NAME} LEFT JOIN {BankServicesConstants.TABLE_NAME} " +
                               $"ON " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SERVICE_ID} = {BankServicesConstants.TABLE_NAME}.{BankServicesConstants.BANK_SERVICE_ID} " +
                               $"WHERE " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.BANK_NAME} = @bankName AND " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SCREEN_ID} = @screenId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, ButtonsConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@screenId", SqlDbType.Int).Value = screenId;

                List<TicketingButton> buttons = new List<TicketingButton>();
                DbUtils.ExecuteReader(command, (reader) => {
                    while (reader.Read()) {
                        var button = ButtonFromReader(reader, bankName, screenId);
                        if (button != null) {
                            buttons.Add(button);
                        }
                    }

                    reader.Close();
                });

                return buttons;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public IEnumerable<TicketingButton> GetButtons(string bankName, int screenId, int branchId) {
            try {
                string query = $"SELECT " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.BANK_NAME}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SCREEN_ID}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.BUTTON_ID}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.TYPE}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.NAME_EN}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.NAME_AR}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SERVICE_ID}, " +
                                   $"{BankServicesConstants.TABLE_NAME}.{BankServicesConstants.NAME_EN} AS {SERVICE_NAME_EN}, " +
                                   $"{BankServicesConstants.TABLE_NAME}.{BankServicesConstants.NAME_AR} AS {SERVICE_NAME_AR}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.MESSAGE_EN}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.MESSAGE_AR} " +
                               $"FROM " +
                                   $"{ButtonsConstants.TABLE_NAME} LEFT JOIN {BankServicesConstants.TABLE_NAME} " +
                               $"ON " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SERVICE_ID} = {BankServicesConstants.TABLE_NAME}.{BankServicesConstants.BANK_SERVICE_ID} " +
                               $"WHERE " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.BANK_NAME} = @bankName AND " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SCREEN_ID} = @screenId AND " +
                                   $"({ButtonsConstants.TABLE_NAME}.{ButtonsConstants.TYPE} = {(int) ButtonsConstants.Types.SHOW_MESSAGE} OR " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SERVICE_ID} IN " +
                                       $"(SELECT " +
                                           $"{ServicesCountersConstants.TABLE_NAME}.{ServicesCountersConstants.BANK_SERVICE_ID} " +
                                       $"FROM " +
                                           $"{ServicesCountersConstants.TABLE_NAME} " +
                                       $"WHERE " +
                                           $"{ServicesCountersConstants.TABLE_NAME}.{ServicesCountersConstants.BANK_NAME} = @servicesCountersBankName AND " +
                                           $"{ServicesCountersConstants.TABLE_NAME}.{ServicesCountersConstants.BRANCH_ID} = @branchId));";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, ButtonsConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@screenId", SqlDbType.Int).Value = screenId;
                command.Parameters.Add("@servicesCountersBankName", SqlDbType.VarChar, ServicesCountersConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branchId;

                List<TicketingButton> buttons = new List<TicketingButton>();
                DbUtils.ExecuteReader(command, (reader) => {
                    while (reader.Read()) {
                        var button = ButtonFromReader(reader, bankName, screenId);
                        if (button != null) {
                            buttons.Add(button);
                        }
                    }

                    reader.Close();
                });

                return buttons;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        public IEnumerable<TicketingButton> GetButtons(string bankName, int screenId, int branchId, int counterId) {
            try {
                string query = $"SELECT " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.BANK_NAME}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SCREEN_ID}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.BUTTON_ID}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.TYPE}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.NAME_EN}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.NAME_AR}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SERVICE_ID}, " +
                                   $"{BankServicesConstants.TABLE_NAME}.{BankServicesConstants.NAME_EN} AS {SERVICE_NAME_EN}, " +
                                   $"{BankServicesConstants.TABLE_NAME}.{BankServicesConstants.NAME_AR} AS {SERVICE_NAME_AR}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.MESSAGE_EN}, " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.MESSAGE_AR} " +
                               $"FROM " +
                                   $"{ButtonsConstants.TABLE_NAME} LEFT JOIN {BankServicesConstants.TABLE_NAME} " +
                               $"ON " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SERVICE_ID} = {BankServicesConstants.TABLE_NAME}.{BankServicesConstants.BANK_SERVICE_ID} " +
                               $"WHERE " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.BANK_NAME} = @bankName AND " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SCREEN_ID} = @screenId AND " +
                                   $"({ButtonsConstants.TABLE_NAME}.{ButtonsConstants.TYPE} = {(int) ButtonsConstants.Types.SHOW_MESSAGE} OR " +
                                   $"{ButtonsConstants.TABLE_NAME}.{ButtonsConstants.SERVICE_ID} IN " +
                                       $"(SELECT " +
                                           $"{ServicesCountersConstants.TABLE_NAME}.{ServicesCountersConstants.BANK_SERVICE_ID} " +
                                       $"FROM " +
                                           $"{ServicesCountersConstants.TABLE_NAME} " +
                                       $"WHERE " +
                                           $"{ServicesCountersConstants.TABLE_NAME}.{ServicesCountersConstants.BANK_NAME} = @servicesCountersBankName AND " +
                                           $"{ServicesCountersConstants.TABLE_NAME}.{ServicesCountersConstants.BRANCH_ID} = @branchId AND " +
                                           $"{ServicesCountersConstants.TABLE_NAME}.{ServicesCountersConstants.COUNTER_ID} = @counterId));";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, ButtonsConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@screenId", SqlDbType.Int).Value = screenId;
                command.Parameters.Add("@servicesCountersBankName", SqlDbType.VarChar, ServicesCountersConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branchId;
                command.Parameters.Add("@counterId", SqlDbType.Int).Value = counterId;

                List<TicketingButton> buttons = new List<TicketingButton>();
                DbUtils.ExecuteReader(command, (reader) => {
                    while (reader.Read()) {
                        var button = ButtonFromReader(reader, bankName, screenId);
                        if (button != null) {
                            buttons.Add(button);
                        }
                    }

                    reader.Close();
                });

                return buttons;
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

        public bool CheckIfCounterExists(string bankName, int branchId, int counterId) {
            try {
                string query = $"SELECT COUNT(*) FROM {CountersConstants.TABLE_NAME} WHERE {CountersConstants.BANK_NAME} = @bankName AND {CountersConstants.BRANCH_ID} = @branchId AND {CountersConstants.COUNTER_ID} = @counterId;";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add("@bankName", SqlDbType.VarChar, CountersConstants.BANK_NAME_SIZE).Value = bankName;
                command.Parameters.Add("@branchId", SqlDbType.Int).Value = branchId;
                command.Parameters.Add("@counterId", SqlDbType.Int).Value = counterId;

                return (int) DbUtils.ExecuteScalar(command) == 1;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }

        private TicketingButton ButtonFromReader(SqlDataReader reader, string bankName, int screenId) {
            try {
                TicketingButton button = null;

                int buttonId = (int) reader[ButtonsConstants.BUTTON_ID];
                ButtonsConstants.Types type = (ButtonsConstants.Types) reader[ButtonsConstants.TYPE];
                string nameEn = (string) reader[ButtonsConstants.NAME_EN];
                string nameAr = (string) reader[ButtonsConstants.NAME_AR];

                if (type == ButtonsConstants.Types.ISSUE_TICKET) {
                    int serviceId;
                    string serviceNameEn, serviceNameAr;
                    try {
                        serviceId = (int) reader[ButtonsConstants.SERVICE_ID];
                        serviceNameEn = (string) reader[SERVICE_NAME_EN];
                        serviceNameAr = (string) reader[SERVICE_NAME_AR];
                    }
                    catch (InvalidCastException) { // service ID is null
                        serviceId = 0;
                        serviceNameEn = string.Empty;
                        serviceNameAr = string.Empty;
                    }

                    button = new IssueTicketButton() {
                        BankName = bankName,
                        ScreenId = screenId,
                        ButtonId = buttonId,
                        Type = type,
                        NameEn = nameEn,
                        NameAr = nameAr,
                        ServiceId = serviceId,
                        ServiceNameEn = serviceNameEn,
                        ServiceNameAr = serviceNameAr,
                    };
                }
                else if (type == ButtonsConstants.Types.SHOW_MESSAGE) {
                    string messageEn = (string) reader[ButtonsConstants.MESSAGE_EN];
                    string messageAr = (string) reader[ButtonsConstants.MESSAGE_AR];

                    button = new ShowMessageButton() {
                        BankName = bankName,
                        ScreenId = screenId,
                        ButtonId = buttonId,
                        Type = type,
                        NameEn = nameEn,
                        NameAr = nameAr,
                        MessageEn = messageEn,
                        MessageAr = messageAr
                    };
                }
                else {
                    WindowsLogsHelper.Log("Unsupported button type", EventLogEntryType.Warning);
                }

                return button;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }

            return default;
        }
    }
}
