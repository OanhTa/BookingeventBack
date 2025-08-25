namespace bookingEvent.Infrastructure.Middlewares
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionAttribute : Attribute
    {
        public string Action { get; }
        public PermissionAttribute(string action)
        {
            Action = action;
        }
    }
}
