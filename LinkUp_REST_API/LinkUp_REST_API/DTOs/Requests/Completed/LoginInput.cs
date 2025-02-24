using LinkUp_REST_API.Util.ValidationDefitions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class LoginInput
    {
        [EmailAddress]
        [StringLength(100)]
        [DefaultValue("example@example.com")]
        public required string Email { get; set; }

        [StrongPassword]
        [StringLength(100)]
        [DefaultValue("Password123!")]
        public required string Password { get; set; }
    }
}
