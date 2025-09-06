using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<OrganisationUser> OrganisationUser { get; set; }

        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventDetail> EventDetail { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Ticket> Ticket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<RolePermission>().HasKey(x => new { x.RoleId, x.PermissionId });
            modelBuilder.Entity<UserPermission>().HasKey(x => new { x.UserId, x.PermissionId });
            modelBuilder.Entity<OrganisationUser>().HasKey(x => new { x.UserId, x.OrganisationId });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Events)
                      .HasForeignKey(e => e.CategoryId);
            });

            modelBuilder.Entity<EventDetail>(entity =>
            {
                entity.HasKey(ed => ed.Id);
                entity.Property(ed => ed.Description)
                      .HasMaxLength(1000);

                entity.HasOne(ed => ed.Event)
                      .WithOne(e => e.EventDetail)
                      .HasForeignKey<EventDetail>(ed => ed.EventId);
            });

            modelBuilder.Entity<TicketType>()
                .HasOne(tt => tt.Event)
                .WithMany(e => e.TicketTypes)
                .HasForeignKey(tt => tt.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.TicketType)
                .WithMany(tt => tt.Tickets)
                .HasForeignKey(t => t.TicketTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany()
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrganisationUser>()
               .HasOne(ou => ou.User)
               .WithMany(u => u.OrganisationUsers)
               .HasForeignKey(ou => ou.UserId)
               .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<OrganisationUser>()
                .HasOne(ou => ou.Organisation)
                .WithMany(o => o.OrganisationUsers)
                .HasForeignKey(ou => ou.OrganisationId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
