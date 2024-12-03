using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class AccountCreateInput
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]       // TODO: should be more complex
        public required string Password { get; set; }

        // PersonInformation
        public required string FirstName { get; set; }
        public required string Surname { get; set; }
        public string? Phone { get; set; }
        public DateOnly BirthDate { get; set; }
        public required string Gender { get; set; }          // TODO: enum

    }

}
