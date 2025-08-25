using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<AccountGroup> AccountGroup { get; set; }
        public DbSet<AccountGroupPermissions> AccountGroupPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Thiết lập Fluent API cho Account
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.HasOne(a => a.AccountGroup)
                      .WithMany(g => g.Accounts)
                      .HasForeignKey(a => a.AccountGroupId);  
            });
         
        }
    }
}
