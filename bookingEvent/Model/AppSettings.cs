using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class AppSettings : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } // Khóa chính duy nhất cho mỗi setting
        public string Name { get; set; } = null!; // Tên setting, ví dụ: "App.Account.IsSelfRegistrationEnabled"
        public string Value { get; set; } = null!; // Giá trị setting (chuỗi: "true"/"false", số, text...)
        public string? ProviderName { get; set; } // Cấp áp dụng: G = Global, U = User
        public string? ProviderKey { get; set; } // Khóa gắn với Provider (TenantId/UserId), null nếu Global

    }
}
