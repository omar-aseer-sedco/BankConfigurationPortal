namespace BankConfigurationPortal.Web.Constants {
    public static class Languages {
        public const string ENGLISH = "en";
        public const string ARABIC = "ar";

        public static bool IsValidLanguageString(string languageString) {
            return languageString == ENGLISH || languageString == ARABIC;
        }
    }
}
