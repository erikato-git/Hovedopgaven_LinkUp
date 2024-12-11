using LinkUp_REST_API.Util.ValidationDefitions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class AccountUpdateInput
    {
        [StringLength(36)]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public required Guid AccountId { get; set; }

        [EmailAddress]
        [StringLength(100)]
        [DefaultValue("example@example.com")]
        public string? Email { get; set; }

        [StrongPassword]
        [StringLength(100)]
        [DefaultValue("Password123!")]
        public required string Password { get; set; }

        [Name]
        [StringLength(100)]
        [DefaultValue("John")]
        public string? FirstName { get; set; }

        [Name]
        [StringLength(100)]
        [DefaultValue("Doe")]
        public string? Surname { get; set; }

        [Phone]
        [StringLength(15)]
        [DefaultValue("+1234567890")]
        public string? Phone { get; set; }

        [DefaultValue("2000-01-01")]
        public DateOnly BirthDate { get; set; }

        [StringLength(50)]
        [DefaultValue("Male")]
        public string? Gender { get; set; }
    }

}
