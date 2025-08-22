using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<NguoiDung> NguoiDung { get; set; }
    }
}
