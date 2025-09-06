using System;

namespace bookingEvent.Infrastructure.Middlewares
{
 
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class PermissionAttribute : Attribute
    {
        public string PermissionName { get; }

        public PermissionAttribute(string permissionName)
        {
            PermissionName = permissionName;
        }
    }
}
