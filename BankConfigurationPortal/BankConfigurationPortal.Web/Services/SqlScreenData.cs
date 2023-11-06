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
                string query = $"SELECT * FROM {ButtonsConstants.TABLE_NAME} WHERE {ButtonsConstants.BANK_NAME} = @bankName AND {ButtonsConstants.SCREEN_ID} = @screenId;";
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
                string query = $"SELECT * FROM {ButtonsConstants.TABLE_NAME} WHERE {ButtonsConstants.BANK_NAME} = @bankName AND {ButtonsConstants.SCREEN_ID} = @screenId AND ({ButtonsConstants.TYPE} = {(int) ButtonsConstants.Types.SHOW_MESSAGE} OR " +
                               $"service_id IN (SELECT {ServicesCountersConstants.BANK_SERVICE_ID} FROM {ServicesCountersConstants.TABLE_NAME} WHERE {ServicesCountersConstants.BANK_NAME} = @servicesCountersBankName AND " +
                               $"{ServicesCountersConstants.BRANCH_ID} = @branchId));";
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
                string query = $"SELECT * FROM {ButtonsConstants.TABLE_NAME} WHERE {ButtonsConstants.BANK_NAME} = @bankName AND {ButtonsConstants.SCREEN_ID} = @screenId AND ({ButtonsConstants.TYPE} = {(int) ButtonsConstants.Types.SHOW_MESSAGE} OR " +
                               $"service_id IN (SELECT {ServicesCountersConstants.BANK_SERVICE_ID} FROM {ServicesCountersConstants.TABLE_NAME} WHERE {ServicesCountersConstants.BANK_NAME} = @servicesCountersBankName AND " +
                               $"{ServicesCountersConstants.BRANCH_ID} = @branchId AND {ServicesCountersConstants.COUNTER_ID} = @counterId));";
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
                    try {
                        serviceId = (int) reader[ButtonsConstants.SERVICE_ID];
                    }
                    catch (InvalidCastException) { // service ID is null
                        serviceId = 0;
                    }

                    button = new IssueTicketButton() {
                        BankName = bankName,
                        ScreenId = screenId,
                        ButtonId = buttonId,
                        Type = type,
                        NameEn = nameEn,
                        NameAr = nameAr,
                        ServiceId = serviceId
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
