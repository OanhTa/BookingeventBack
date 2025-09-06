namespace bookingEvent.Const
{
    public class Permissions
    {
        public static class Users
        {
            public const string Create = "Identity.Users.Create";
            public const string Read = "Identity.Users.Read";
            public const string Update = "Identity.Users.Update";
            public const string Delete = "Identity.Users.Delete";
        }

        // ===== Roles =====
        public static class Roles
        {
            public const string Create = "Identity.Roles.Create";
            public const string Read = "Identity.Roles.Read";
            public const string Update = "Identity.Roles.Update";
            public const string Delete = "Identity.Roles.Delete";
            public const string Manage = "Identity.Roles.Manage"; 
        }

        // ===== Categories =====
        public static class Categories
        {
            public const string Create = "Identity.Categories.Create";
            public const string Read = "Identity.Categories.Read";
            public const string Update = "Identity.Categories.Update";
            public const string Delete = "Identity.Categories.Delete";
        }

        // ===== Events =====
        public static class Events
        {
            public const string Create = "Identity.Events.Create";
            public const string Read = "Identity.Events.Read";
            public const string Update = "Identity.Events.Update";
            public const string Delete = "Identity.Events.Delete";
            public const string Book = "Identity.Events.Book";   
        }

        // ===== Audit Logs =====
        public static class AuditLogs
        {
            public const string Read = "AuditLogs.Read";
            public const string Export = "AuditLogs.Export";
        }
    }
}
