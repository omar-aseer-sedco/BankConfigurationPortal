namespace BankConfigurationPortal.Data.Constants {
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

        public const int BANK_NAME_SIZE = 255;
    }
}
