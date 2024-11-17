using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace REST_API.DTOs.AccountDomain
{
    /*
     * Only Guid is required, other properties doesn't need to be updated if they're not specified
     */
    public class UpdateAccountDTO
    {
        [Required(ErrorMessage = "Account ID is required.")]
        public Guid AccountId { get; set; }

        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters.")]
        public string? FirstName { get; set; }

        [StringLength(100, ErrorMessage = "Surname cannot be longer than 100 characters.")]
        public string? Surname { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters.")]
        public string? Phone { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? BirthDate { get; set; }

        [StringLength(10, ErrorMessage = "Gender cannot be longer than 10 characters.")]
        public string? Gender { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
