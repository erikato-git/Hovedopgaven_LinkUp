using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests
{
    public class PersonInformationCreateDTO
    {
        [RegularExpression(@"^[A-Za-z]+(?: [A-Za-z]+)*$", ErrorMessage = "First name should only contain letters and spaces.")]
        public required string FirstName { get; set; }

        [RegularExpression(@"^[A-Za-z]+(?: [A-Za-z]+)*$", ErrorMessage = "Surname should only contain letters and spaces.")]
        public required string Surname { get; set; }

        [Phone]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number is not in a valid format.")]
        public string? Phone { get; set; }

        public required DateOnly BirthDate { get; set; }

        [RegularExpression(@"^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public required string Gender { get; set; }
    }
}
