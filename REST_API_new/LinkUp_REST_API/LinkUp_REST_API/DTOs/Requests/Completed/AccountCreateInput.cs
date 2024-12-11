using LinkUp_REST_API.Util.ValidationDefitions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class AccountCreateInput
    {
        [EmailAddress]
        [StringLength(100)]
        [DefaultValue("example@example.com")]
        public required string Email { get; set; }

        [StrongPassword]
        [StringLength(100)]
        [DefaultValue("Password123!")]
        public required string Password { get; set; }

        // PersonInformation
        [Name]
        [StringLength(100)]
        [DefaultValue("John")]
        public required string FirstName { get; set; }

        [Name]
        [StringLength(100)]
        [DefaultValue("Doe")]  
        public required string Surname { get; set; }

        [Phone]
        [StringLength(15)]
        [DefaultValue("+1234567890")]
        public string? Phone { get; set; }

        [DefaultValue("2000-01-01")]
        public DateOnly BirthDate { get; set; }

        [StringLength(50)]
        [DefaultValue("Male")] 
        public required string Gender { get; set; }
    }

}
