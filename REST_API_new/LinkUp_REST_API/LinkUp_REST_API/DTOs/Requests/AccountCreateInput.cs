using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests
{
    public class AccountCreateInput
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]       // TODO: should be more complex
        public required string Password { get; set; }

        public required PersonInformationCreateInput PersonInformation { get; set; }
    }

}
