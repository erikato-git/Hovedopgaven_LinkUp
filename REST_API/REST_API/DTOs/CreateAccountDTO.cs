using System.ComponentModel.DataAnnotations;

namespace REST_API.DTOs
{
    public class CreateAccountDTO
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(100, ErrorMessage = "Surname cannot be longer than 100 characters.")]
        public String Surname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters.")]
        public String Phone { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10, ErrorMessage = "Gender cannot be longer than 10 characters.")]
        public String Gender { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}
