using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Pending
{
    public class PersonInformationCreateInput
    {
        [RegularExpression(@"^[A-Za-z]+(?: [A-Za-z]+)*$", ErrorMessage = "First name should only contain letters and spaces.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string? FirstName { get; set; }

        [RegularExpression(@"^[A-Za-z]+(?: [A-Za-z]+)*$", ErrorMessage = "Surname should only contain letters and spaces.")]
        [StringLength(50, ErrorMessage = "Surname cannot be longer than 50 characters.")]
        public string? Surname { get; set; }

        [Phone]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number is not in a valid format.")]
        public string? Phone { get; set; }

        public required DateOnly BirthDate { get; set; }

        [RegularExpression(@"^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public required string Gender { get; set; }
    }
}
