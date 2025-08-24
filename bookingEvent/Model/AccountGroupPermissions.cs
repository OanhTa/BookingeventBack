using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    [Keyless]
    public class AccountGroupPermissions
    {
        public Guid AccountGroupId { get; set; }
        public String FormName { get; set; }
        public String Action { get; set; }
        public bool AllowAction { get; set; }
    }
}
