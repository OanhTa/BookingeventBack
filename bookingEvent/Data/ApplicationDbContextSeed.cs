using bookingEvent.Model;
using bookingEvent.Const;

namespace bookingEvent.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var permissions = new List<Permission>
                {

                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Users.Create, Description = "Create users" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Users.Read, Description = "View users" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Users.Update, Description = "Update users" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Users.Delete, Description = "Delete users" },

                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Roles.Create, Description = "Create roles" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Roles.Read, Description = "View roles" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Roles.Update, Description = "Update roles" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Roles.Delete, Description = "Delete roles" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Roles.Manage, Description = "Manage roles" },

                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Categories.Create, Description = "Create categories" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Categories.Read, Description = "View categories" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Categories.Update, Description = "Update categories" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Categories.Delete, Description = "Delete categories" },

                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Events.Create, Description = "Create events" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Events.Read, Description = "View events" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Events.Update, Description = "Update events" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Events.Delete, Description = "Delete events" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.Events.Book, Description = "Book events" },

                    new Permission { Id = Guid.NewGuid(), Name = Permissions.AuditLogs.Read, Description = "View audit logs" },
                    new Permission { Id = Guid.NewGuid(), Name = Permissions.AuditLogs.Export, Description = "Export audit logs" }
                };

            foreach (var perm in permissions)
            {
                if (!context.Permissions.Any(p => p.Name == perm.Name))
                {
                    context.Permissions.Add(perm);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}