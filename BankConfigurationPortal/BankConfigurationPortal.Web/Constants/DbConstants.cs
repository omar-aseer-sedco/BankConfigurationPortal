﻿namespace BankConfigurationPortal.Web.Constants {
    public static class BanksConstants {
        public const string TABLE_NAME = "Banks";

        public const string BANK_NAME = "bank_name";
        public const string PASSWORD = "password";

        public const int BANK_NAME_SIZE = 255;
        public const int PASSWORD_SIZE = 255;
    }

    public static class UsersConstants {
        public const string TABLE_NAME = "Users";

        public const string USERNAME = "username";
        public const string BANK_NAME = "bank_name";
        public const string PASSWORD = "password";

        public const int USERNAME_SIZE = 255;
        public const int BANK_NAME_SIZE = 255;
        public const int PASSWORD_SIZE = 255;
    }

    public static class BranchesConstants {
        public const string TABLE_NAME = "Branches";

        public const string BANK_NAME = "bank_name";
        public const string BRANCH_ID = "branch_id";
        public const string NAME_EN = "name_en";
        public const string NAME_AR = "name_ar";
        public const string ACTIVE = "active";

        public const int BANK_NAME_SIZE = 255;
        public const int NAME_EN_SIZE = 100;
        public const int NAME_AR_SIZE = 100;
    }

    public static class BankServicesConstants {
        public const string TABLE_NAME = "BankServices";

        public const string BANK_NAME = "bank_name";
        public const string BANK_SERVICE_ID = "service_id";
        public const string NAME_EN = "name_en";
        public const string NAME_AR = "name_ar";
        public const string ACTIVE = "active";
        public const string MAX_DAILY_TICKETS = "max_daily_tickets";
        public const string MIN_SERVICE_TIME = "min_service_time";
        public const string MAX_SERVICE_TIME = "max_service_time";

        public const int BANK_NAME_SIZE = 255;
        public const int NAME_EN_SIZE = 100;
        public const int NAME_AR_SIZE = 100;
    }

    public static class CountersConstants {
        public const string TABLE_NAME = "Counters";

        public const string BANK_NAME = "bank_name";
        public const string BRANCH_ID = "branch_id";
        public const string COUNTER_ID = "counter_id";
        public const string NAME_EN = "name_en";
        public const string NAME_AR = "name_ar";
        public const string ACTIVE = "active";
        public const string TYPE = "type";

        public const int BANK_NAME_SIZE = 255;
        public const int NAME_EN_SIZE = 100;
        public const int NAME_AR_SIZE = 100;
    }

    public static class ServicesCountersConstants {
        public const string TABLE_NAME = "ServicesCounters";

        public const string BANK_NAME = "bank_name";
        public const string BRANCH_ID = "branch_id";
        public const string COUNTER_ID = "counter_id";
        public const string BANK_SERVICE_ID = "service_id";

        public const string ADD_SERVICES_TO_COUNTER_PROCEDURE = "AddServicesToCounters";
        public const string ADD_SERVICES_TO_COUNTER_PARAMETER_TYPE = "dbo.add_service_counter_parameter";
        public const string ADD_SERVICES_TO_COUNTER_PARAMETER_NAME = "@service_counters";

        public const int BANK_NAME_SIZE = 255;
    }

    public static class SessionsConstants {
        public const string TABLE_NAME = "Sessions";

        public const string USERNAME = "username";
        public const string SESSION_ID = "session_id";
        public const string EXPIRES = "expires";
        public const string USER_AGENT = "user_agent";
        public const string IP_ADDRESS = "ip_address";

        public const int USERNAME_SIZE = 255;
        public const int USER_AGENT_SIZE = 255;
        public const int IP_ADDRESS_SIZE = 39;
    }

    public static class ScreensConstants {
        public const string TABLE_NAME = "TicketingScreens";
        public const string BANK_NAME = "bank_name";
        public const string SCREEN_ID = "screen_id";
        public const string IS_ACTIVE = "is_active";
        public const string SCREEN_TITLE = "screen_title";

        public const int BANK_NAME_SIZE = 255;
        public const int SCREEN_TITLE_SIZE = 255;
    }

    public static class ButtonsConstants {
        public const string TABLE_NAME = "TicketingButtons";
        public const string BANK_NAME = "bank_name";
        public const string SCREEN_ID = "screen_id";
        public const string BUTTON_ID = "button_id";
        public const string TYPE = "type";
        public const string NAME_EN = "name_en";
        public const string NAME_AR = "name_ar";
        public const string SERVICE_ID = "service_id";
        public const string MESSAGE_EN = "message_en";
        public const string MESSAGE_AR = "message_ar";

        public const int BANK_NAME_SIZE = 255;
        public const int NAME_EN_SIZE = 255;
        public const int NAME_AR_SIZE = 255;
        public const int SERVICE_SIZE = 255;
        public const int MESSAGE_EN_SIZE = 1000;
        public const int MESSAGE_AR_SIZE = 1000;

        public enum Types {
            UNDEFINED = 0,
            ISSUE_TICKET = 1,
            SHOW_MESSAGE = 2,
        }
    }
}
