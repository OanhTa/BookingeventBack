namespace bookingEvent.DTO
{
    public class AccountDTO
    {
        public Guid? Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public AccountGroupDTO? AccountGroup { get; set; }
    }
}
