namespace bookingEvent.Const
{
    public class AppSettingNames
    {
        public const string IsSelfRegistrationEnabled = "App.Account.IsSelfRegistrationEnabled";
        public const string EnableLocalLogin = "App.Account.EnableLocalLogin";
        public const string PreventEmailEnumeration = "App.Account.PreventEmailEnumeration";
 
        public const string PasswordMinLength = "App.PasswordPolicy.MinLength";
        public const string PasswordUniqueChars = "App.PasswordPolicy.UniqueChars";
        public const string PasswordRequireNonAlphanumeric = "App.PasswordPolicy.RequireNonAlphanumeric";
        public const string PasswordRequireLowercase = "App.PasswordPolicy.RequireLowercase";
        public const string PasswordRequireUppercase = "App.PasswordPolicy.RequireUppercase";
        public const string PasswordRequireDigit = "App.PasswordPolicy.RequireDigit";

        public const string LockoutDuration = "App.Identity.LockoutDuration";
        public const string MaxFailedAccess = "App.Identity.MaxFailedAccess";



    }
}
